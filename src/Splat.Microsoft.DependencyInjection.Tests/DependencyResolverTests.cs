// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
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
        /// Shoulds the resolve views.
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
        /// Shoulds the resolve views.
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
    }
}
