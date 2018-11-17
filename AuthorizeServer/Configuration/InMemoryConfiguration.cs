using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.Configuration
{
    public static class InMemoryConfiguration
    {
        /// <summary>
        /// 定义api资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[] { new ApiResource("Test", "apiresource") };
        }
        /// <summary>
        ///  定义客户端 基于密码和客户端信任
        /// </summary>
        public static IEnumerable<Client> Clients()
        {
            return new[] {
                new Client{
                    ClientId ="mvc",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes=new[]{
                        "Test"
                    }
                }
            };
        }
        public static IEnumerable<TestUser> Users()
        {
            return new[] { 
                new TestUser
                {
                    SubjectId=Guid.NewGuid().ToString(),
                    Username="admin",
                    Password="qwe"
                },
                 new TestUser
                {
                    SubjectId=Guid.NewGuid().ToString(),
                    Username="manager",
                    Password="qwe"
                },
                  new TestUser
                {
                    SubjectId=Guid.NewGuid().ToString(),
                    Username="guest",
                    Password="qwe"
                },
            };
        }
    }
}
