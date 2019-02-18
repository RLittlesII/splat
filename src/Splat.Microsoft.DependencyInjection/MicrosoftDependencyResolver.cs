// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.DependencyInjection
{
    /// <summary>
    /// Microsoft Dependency Injection implementation for <see cref="IMutableDependencyResolver"/>.
    /// </summary>
    public class MicrosoftDependencyResolver : IMutableDependencyResolver
    {
        private readonly Dictionary<string, ServiceDescriptor> _namedInstanceFactory;
        private IServiceCollection _serviceDescriptors;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver"/> class.
        /// </summary>
        /// <param name="serviceDescriptors">The service descriptors.</param>
        public MicrosoftDependencyResolver(IServiceCollection serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
            _namedInstanceFactory = new Dictionary<string, ServiceDescriptor>();
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            _serviceProvider = _serviceDescriptors.BuildServiceProvider();

            if (contract != null && !_namedInstanceFactory.ContainsKey(contract))
            {
                return _serviceProvider.GetService(_namedInstanceFactory[contract].GetType());
            }

            return _serviceProvider.GetService(serviceType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            _serviceProvider = _serviceDescriptors.BuildServiceProvider();

            if (contract != null && !_namedInstanceFactory.ContainsKey(contract))
            {
                return _serviceProvider.GetServices(_namedInstanceFactory[contract].GetType());
            }

            return _serviceProvider.GetServices(serviceType);
        }

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (contract != null && !_namedInstanceFactory.ContainsKey(contract))
            {
                _namedInstanceFactory.Add(contract, new ServiceDescriptor(serviceType, factory()));
                _serviceDescriptors.AddScoped(serviceType, provider => factory());
            }
            else
            {
                _serviceDescriptors.AddScoped(serviceType, provider => factory);
            }
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            if (contract != null && _namedInstanceFactory.ContainsKey(contract))
            {
                _serviceDescriptors.Remove(_namedInstanceFactory[contract]);
            }
            else
            {
                _serviceDescriptors.Remove(new ServiceDescriptor(serviceType, _serviceProvider.GetService(serviceType)));
            }
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            if (contract != null && _namedInstanceFactory.ContainsKey(contract))
            {
                _serviceDescriptors.Remove(_namedInstanceFactory[contract]);
            }
            else
            {
                _serviceDescriptors.Remove(new ServiceDescriptor(serviceType, _serviceProvider.GetService(serviceType)));
            }
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        /// <param name="disposing">Whether or not the instance is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceDescriptors = null;
                _serviceProvider = null;
            }
        }
    }
}
