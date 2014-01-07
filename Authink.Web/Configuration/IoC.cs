using System.Collections.Generic;
using System.Web;
using Authink.Data.ResourceFileStorage;
using Authink.Data.ResourceFileStorage.Configuration;
using Authink.Web.Models.Picture;

namespace Authink.Web.DependencyResolution {
    
    using StructureMap;

    using Authink.Core.Model.Queries;
    using Authink.Core.Model.Queries.Impl;
    using Authink.Core.Model.Commands;
    using Authink.Core.Model.Commands.Impl;
    using Authink.Core.Model.Services;
    using Authink.Core.Model.Services.Impl;
    using NLog;

    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.Assembly("Authink.Core");
                                        scan.Assembly("Authink.Data");
                                        scan.SingleImplementationsOfInterface();
                                    });

                            x.For<HttpContextBase>().HttpContextScoped().Use(() => new HttpContextWrapper(HttpContext.Current));
                            x.For<List<HttpPostedFileBase>>().Use(() => new List<HttpPostedFileBase>());
                            x.For<List<ColorData>>().Use(() => new List<ColorData>());
                            x.For<List<string>>().Use(() => new List<string>());
                            x.For<Logger>().Use(context => LogManager.GetLogger("AuThink"));

                            x.For<ResourceFileStorageAdapter.Settings>().Use(() => ResourceFileStorageAdapterConfigurationSection.GetSettings());
                        });

            return ObjectFactory.Container;
        }
    }
}