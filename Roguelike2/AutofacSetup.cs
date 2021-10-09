using Autofac;
using log4net;
using log4net.Config;
using Roguelike2.Entities;
using Roguelike2.Logging;
using Roguelike2.Serialization;
using Roguelike2.Serialization.Settings;
using Roguelike2.Ui;

namespace Roguelike2
{
    public static class AutofacSetup
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UiManager>()
                .As<IUiManager>()
                .SingleInstance();
            builder.RegisterType<GameManager>()
                .As<IGameManager>()
                .SingleInstance();
            builder.RegisterType<SaveManager>()
                .As<ISaveManager>()
                .SingleInstance();

            builder.RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterType<AppSettings>()
                .As<IAppSettings>()
                .SingleInstance();

            //log4net
            builder.Register(
                _ =>
                {
                    XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
                    return LogManager.GetLogger(typeof(Rl2));
                })
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<Rl2>().AsSelf();
            return builder.Build();
        }
    }
}
