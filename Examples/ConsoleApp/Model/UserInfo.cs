using System;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp.Model
{
    public class UserInfo
    {
        public UserInfo(string login, int id, string avatarUrl, string url, string name, DateTime createdAt, DateTime updatedAt)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Id = id;
            AvatarUrl = avatarUrl ?? throw new ArgumentNullException(nameof(avatarUrl));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Name = name ?? string.Empty;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public string Login { get; }

        public int Id { get; }

        public string AvatarUrl { get; }

        public string Url { get; }

        public string Name { get; }

        public DateTime CreatedAt { get; }

        public DateTime UpdatedAt { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                sb.AppendFormat("{0}={1}", descriptor.Name, descriptor.GetValue(this)).AppendLine();
            }
            return sb.ToString();
        }
    }
}