using AutoMapper;
using MHPQ.Authorization.Users;
using MHPQ.EntityDb;
using Microsoft.Exchange.WebServices.Data;

namespace MHPQ
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings(IMapperConfigurationExpression mapper)
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal(mapper);

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal(IMapperConfigurationExpression mapper)
        {
            //mapper.CreateMap<User, UserEditDto>()
            //    .ForMember(dto => dto.Password, options => options.Ignore())
            //    .ReverseMap()
            //    .ForMember(user => user.Password, options => options.Ignore());

            mapper.CreateMap<WebPushEndPoint, PushSubscription>();
                 
        }
    }
}