using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_SignalR.Models;

namespace Test_SignalR.Data
{
    public static class SeedData
    {
        public static List<User> Users = new List<User>()
        {
            new User()
            {
                ID = 1,
                UserName = "test",
                Password = "1"
            },
            new User()
            {
                ID = 2,
                UserName = "fillin",
                Password = "1"
            },
            new User()
            {
                ID = 3,
                UserName = "张三",
                Password = "1"
            },
            new User()
            {
                ID = 4,
                UserName = "李四",
                Password = "1"
            },
            new User()
            {
                ID = 5,
                UserName = "王五",
                Password = "1"
            },
            new User()
            {
                ID = 6,
                UserName = "赵六",
                Password = "1"
            },
        };
    }
}
