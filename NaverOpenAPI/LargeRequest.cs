// This source code is a part of NAVER Open API Wrapper.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NaverOpenAPI
{
    /// <summary>
    /// 들어온 입력을 순서대로 처리하는 요청 큐를 구현합니다.
    /// </summary>
    internal class LargeRequest
    {
        int thread_count = 0;
        int busy_thread = 0;
        int capacity = 0;
        Session sess;

        Queue<(string, string, Action<string>)> queue = new Queue<(string, string, Action<string>)>();
        List<Thread> threads = new List<Thread>();
        List<ManualResetEvent> interrupt = new List<ManualResetEvent>();
        object notify_lock = new object();

        public LargeRequest(Session sess, int capacity = 0)
        {
            this.sess = sess;
            this.capacity = capacity;

            if (this.capacity == 0)
                this.capacity = Environment.ProcessorCount;

            thread_count = this.capacity;

            for (int i = 0; i < this.capacity; i++)
            {
                interrupt.Add(new ManualResetEvent(false));
                threads.Add(new Thread(new ParameterizedThreadStart(remote_thread_handler)));
                threads.Last().Start(i);
            }
        }

        public async Task<string> RequestAsync(string url, string data)
        {
            return await Task.Run(() =>
            {
                var interrupt = new ManualResetEvent(false);
                string result = null;

                add(url, data, (string str) =>
                {
                    result = str;
                    interrupt.Set();
                });

                interrupt.WaitOne();

                return result;
            }).ConfigureAwait(false);
        }

        void notify()
        {
            interrupt.ForEach(x => x.Set());
        }

        void add(string url, string data, Action<string> callback)
        {
            lock (queue) queue.Enqueue((url, data, callback));
            lock (notify_lock) notify();
        }

        private void remote_thread_handler(object i)
        {
            int index = (int)i;

            while (true)
            {
                interrupt[index].WaitOne();

                (string, string, Action<string>) task;

                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        task = queue.Dequeue();
                    }
                    else
                    {
                        interrupt[index].Reset();
                        continue;
                    }
                }

                Interlocked.Increment(ref busy_thread);

                var request = (HttpWebRequest)WebRequest.Create(task.Item1);

                request.Headers.Add("X-Naver-Client-Id", sess.ClientId);
                request.Headers.Add("X-Naver-Client-Secret", sess.ClientSecret);

                request.Method = "POST";
                request.ContentType = "application/json";

                var request_stream = new StreamWriter(request.GetRequestStream());
                request_stream.Write(task.Item2);
                request_stream.Close();

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream istream = response.GetResponseStream();
                            MemoryStream ostream = new MemoryStream();

                            byte[] buffer = new byte[131072];
                            long byte_read;

                            do
                            {
                                byte_read = istream.Read(buffer, 0, buffer.Length);
                                ostream.Write(buffer, 0, (int)byte_read);
                            } while (byte_read != 0);

                            task.Item3(Encoding.UTF8.GetString(ostream.ToArray()));

                            ostream.Close();
                            istream.Close();
                        }
                    }
                }
                catch (WebException e)
                {
                    var response = (HttpWebResponse)e.Response;
                }
                catch (UriFormatException e)
                {
                }
                catch (Exception e)
                {
                }

                Interlocked.Decrement(ref busy_thread);
            }
        }
    }
}
