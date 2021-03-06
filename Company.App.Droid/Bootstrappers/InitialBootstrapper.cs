﻿using Android.App;
using Android.OS;
using Company.App.Application.Bootstrappers;
using Company.App.Common;
using Company.App.Common.Bootstrappers;
using Company.App.Infrastructure.Bootstrappers;
using Company.App.Presentation.Bootstrappers;
using FlexiMvvm.Bootstrappers;
using FlexiMvvm.Configuration;
using FlexiMvvm.Ioc;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;

namespace Company.App.Droid.Bootstrappers
{
    public static class InitialBootstrapper
    {
        public static void Execute(Activity activity, Bundle bundle)
        {
            SetupCrashReporting();
            SetupPlugins(activity, bundle);
            SetupFlexiMvvm();

            var config = CreateBootstrapperConfig();
            ExecuteBootstrappers(config);
        }

        private static void SetupCrashReporting()
        {
#if QA_ENV
            AppCenter.Start(typeof(Crashes));
#else
            AppCenter.Start(typeof(Crashes));
#endif
        }

        private static void SetupPlugins(Activity activity, Bundle bundle)
        {
            CrossCurrentActivity.Current.Init(activity, bundle);
        }

        private static void SetupFlexiMvvm()
        {
            FlexiMvvmConfig.Instance.ShouldRaisePropertyChanged(false);
        }

        private static BootstrapperConfig CreateBootstrapperConfig()
        {
            var config = new BootstrapperConfig();
            config.SetSimpleIoc(new SimpleIoc());
#if DEBUG
            config.SetAppExecutionEnvironment(AppExecutionEnvironment.Dev);
#elif QA_ENV
            config.SetAppExecutionEnvironment(AppExecutionEnvironment.Qa);
#else
            config.SetAppExecutionEnvironment(AppExecutionEnvironment.Prod);
#endif

            return config;
        }

        private static void ExecuteBootstrappers(BootstrapperConfig config)
        {
            var compositeBootstrapper = new CompositeBootstrapper(
                new CommonBootstrapper(),
                new InfrastructureBootstrapper(),
                new PlatformInfrastructureBootstrapper(),
                new ApplicationBootstrapper(),
                new PresentationBootstrapper(),
                new PlatformBootstrapper());
            compositeBootstrapper.Execute(config);
        }
    }
}
