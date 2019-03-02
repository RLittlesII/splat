// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Splat.Common.Test;
using Xunit;

namespace Splat.Microsoft.DependencyInjection.Tests
{
    /// <summary>
    /// Tests to show the <see cref="MicrosoftDependencyResolver"/> works correctly.
    /// </summary>
    public class DependencyResolverTests
    {
        /// <summary>
        /// Should resolve views.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Resolve_Views()
        {
            var services = new ServiceCollection();
            services.AddScoped(typeof(IViewFor<ViewModelOne>), typeof(ViewOne));
            services.AddScoped(typeof(IViewFor<ViewModelTwo>), typeof(ViewTwo));
            services.UseMicrosoftDependencyResolver();

            var viewOne = Locator.Current.GetService(typeof(IViewFor<ViewModelOne>));
            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>));

            viewOne.ShouldNotBeNull();
            viewOne.ShouldBeOfType<ViewOne>();
            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
        }

        /// <summary>
        /// Should resolve named views.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Resolve_Named_View()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register(() => new ViewTwo(), typeof(IViewFor<ViewModelTwo>), "Other");
            Locator.CurrentMutable.Register(() => new ViewOne(), typeof(IViewFor<ViewModelOne>), "View");

            var viewTwo = Locator.Current.GetService(typeof(IViewFor<ViewModelTwo>), "Other");

            viewTwo.ShouldNotBeNull();
            viewTwo.ShouldBeOfType<ViewTwo>();
            viewTwo.ShouldNotBeOfType<ViewOne>();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_UnregisterCurrent()
        {
            var services = new ServiceCollection();
            services.AddScoped(typeof(IScreen), typeof(MockScreen));
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen());

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen));

            screen = Locator.Current.GetService<IScreen>();

            screen.ShouldBeNull();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_UnregisterCurrent_Named()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen1");

            var screen = Locator.Current.GetService<IScreen>("screen1");

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen), "screen1");

            screen = Locator.Current.GetService<IScreen>("screen1");

            screen.ShouldBeNull();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Not_UnregisterCurrent_If_Not_Named()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen1");
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen2");

            var screen1 = Locator.Current.GetService<IScreen>("screen1");
            var screen2 = Locator.Current.GetService<IScreen>("screen2");

            screen1.ShouldBeOfType<MockScreen>();
            screen2.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterCurrent(typeof(IScreen), "screen2");

            Locator.Current.GetService<IScreen>("screen1").ShouldBeOfType<MockScreen>();
            screen2 = Locator.Current.GetService<IScreen>("screen2");
            screen2.ShouldBeNull();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_UnregisterAll()
        {
            var services = new ServiceCollection();
            services.AddScoped(typeof(IScreen), typeof(MockScreen));
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen());

            var screen = Locator.Current.GetService<IScreen>();

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen));

            screen = Locator.Current.GetService<IScreen>();

            screen.ShouldBeNull();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_UnregisterAll_Named()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen1");

            var screen = Locator.Current.GetService<IScreen>("screen1");

            screen.ShouldNotBeNull();
            screen.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen), "screen1");

            screen = Locator.Current.GetService<IScreen>("screen1");

            screen.ShouldBeNull();
        }

        /// <summary>
        /// Should unregister all.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Not_UnregisterAll_If_Not_Named()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen1");
            Locator.CurrentMutable.Register<IScreen>(() => new MockScreen(), "screen2");

            var screen1 = Locator.Current.GetService<IScreen>("screen1");
            var screen2 = Locator.Current.GetService<IScreen>("screen2");

            screen1.ShouldBeOfType<MockScreen>();
            screen2.ShouldBeOfType<MockScreen>();

            Locator.CurrentMutable.UnregisterAll(typeof(IScreen), "screen2");

            Locator.Current.GetService<IScreen>("screen1").ShouldBeOfType<MockScreen>();
            screen2 = Locator.Current.GetService<IScreen>("screen2");
            screen2.ShouldBeNull();
        }

        /// <summary>
        /// Should throw an exception if service registration call back called.
        /// </summary>
        [Fact]
        public void MicrosoftDependencyResolver_Should_Throw_If_ServiceRegistrationCallback_Called()
        {
            var services = new ServiceCollection();
            services.UseMicrosoftDependencyResolver();

            var result = Record.Exception(() =>
                Locator.CurrentMutable.ServiceRegistrationCallback(typeof(IScreen), disposable => { }));

            result.ShouldBeOfType<NotImplementedException>();
        }
    }
}
