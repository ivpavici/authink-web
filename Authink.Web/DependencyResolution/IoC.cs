using System.Collections.Generic;
using System.Web;
using Authink.Web.Models.Picture;
using Authink.Web.Models.Task;

namespace Authink.Web.DependencyResolution {
    
    using StructureMap;

    using Authink.Core.Model.Queries;
    using Authink.Core.Model.Queries.Impl;
    using Authink.Core.Model.Commands;
    using Authink.Core.Model.Commands.Impl;
    using Authink.Core.Model.Services;
    using Authink.Core.Model.Services.Impl;

    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            //x.Scan(scan =>
                            //        {
                            //            scan.TheCallingAssembly();
                            //            scan.WithDefaultConventions();
                            //        });
                            x.For<IUserQueries>().Use<UserQueriesImpl>();
                            x.For<ITaskQueries>().Use<TaskQueriesImpl>();
                            x.For<ITestQueries>().Use<TestQueriesImpl>();
                            x.For<IPictureQueries>().Use<PictureQueriesImpl>();
                            x.For<ISoundQueries>().Use<SoundQueriesImpl>();
                            x.For<IChildQueries>().Use<ChildQueriesImpl>();
                            x.For<IStatisticsQueries>().Use<StatisticsQueriesImpl>();

                            x.For<IUserCommands>().Use<UserCommandsImpl>();
                            x.For<ITaskCommands>().Use<TaskCommandsImpl>();
                            x.For<ITestCommands>().Use<TestCommandsImpl>();
                            x.For<IPictureCommands>().Use<PictureCommandsImpl>();
                            x.For<ISoundCommands>().Use<SoundCommandsImpl>();
                            x.For<IChildCommands>().Use<ChildCommandsImpl>();
                            x.For<IStatisticsCommands>().Use<StatisticsCommandsImpl>();

                            x.For<IFileSystemUtilities>().Use<FileSystemUtilities>();
                            x.For<ISoundServices>().Use<SoundServicesImpl>();
                            x.For<IPictureServices>().Use<PictureServicesImpl>();
                            x.For<ILoginServices>().Use<LoginServicesImpl>();
                            x.For<IUserAccessRights>().Use<UserAccessRightsImpl>();

                            x.For<HttpContextBase>().HttpContextScoped().Use(() => new HttpContextWrapper(HttpContext.Current));
                            x.For<HttpServerUtilityBase>().HttpContextScoped().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
                            x.For<List<HttpPostedFileBase>>().Use(() => new List<HttpPostedFileBase>());
                            x.For<List<ColorData>>().Use(() => new List<ColorData>());
                            x.For<Color>().Use(() => new Color());
                            x.For<List<Color>>().Use(() => new List<Color>());
                            x.For<List<string>>().Use(() => new List<string>());
                        });
            return ObjectFactory.Container;
        }
    }
}