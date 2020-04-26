// This source code is a part of NAVER Open API Wrapper.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;

namespace NaverOpenAPI
{
    public class Session
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public Session CreateSession(string client_id, string client_secret)
        {
            return new Session() { ClientId = client_id, ClientSecret = client_secret };
        }
    }
}
