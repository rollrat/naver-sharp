// This source code is a part of NAVER Open API Wrapper.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace NaverOpenAPI.NaverAPI
{
    public interface IMethodNameAttribute
    {
        string MethodName { get; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandAttribute : Attribute, IMethodNameAttribute
    {
        public CommandAttribute(string methodName)
        {
            this.MethodName = methodName;
        }

        public string MethodName { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandResponseAttribute : Attribute, IMethodNameAttribute
    {
        public CommandResponseAttribute(string methodName)
        {
            this.MethodName = methodName;
        }

        public string MethodName { get; private set; }
    }

    public interface ICommand<T>
    {
    }
}
