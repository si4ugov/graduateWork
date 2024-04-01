using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace withDBSql
{
    internal class UserGroup
    {
        public string Course { get; set; }
        public List<User> Users { get; set; }

        public UserGroup(string course)
        {
            Course = course;
            Users = new List<User>();
        }

        public UserGroup(List<User> users, string course)
        {
            Users = users;
            Course = course;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        // Метод для получения списка групп
        public static List<UserGroup> GetGroups(List<User> users)
        {
            var groups = new List<UserGroup>();
            var courseList = users.Select(u => u.Kurs).Distinct();

            foreach (var course in courseList)
            {
                var group = new UserGroup(course);
                foreach (var user in users.Where(u => u.Kurs == course))
                {
                    group.AddUser(user);
                }
                groups.Add(group);
            }

            return groups;
        }

        // Метод для добавления пользователей в соответствующую группу
        public static void AddUserToGroup(List<UserGroup> groups, User user)
        {
            var group = groups.FirstOrDefault(g => g.Course == user.Kurs);
            if (group == null)
            {
                group = new UserGroup(user.Kurs);
                groups.Add(group);
            }
            group.AddUser(user);
        }
    }
}
