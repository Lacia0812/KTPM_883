using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASC.Tests.TestUtilities
{
    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionFactory = new Dictionary<string, byte[]>();

        // Xác nhận session có khả dụng không
        public bool IsAvailable => true;

        // Lấy ID của session (tạo một ID giả lập)
        public string Id => Guid.NewGuid().ToString();

        // Lấy danh sách các keys trong session
        public IEnumerable<string> Keys => _sessionFactory.Keys;

        // Xóa tất cả session
        public void Clear()
        {
            _sessionFactory.Clear();
        }

        // Commit các thay đổi (không cần thiết trong trường hợp này)
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        // Load các giá trị session (không cần thiết trong trường hợp này)
        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        // Xóa một giá trị khỏi session
        public void Remove(string key)
        {
            _sessionFactory.Remove(key);
        }

        // Lưu giá trị vào session (chuyển đối tượng thành byte[])
        public void Set(string key, byte[] value)
        {
            _sessionFactory[key] = value;
        }

        // Lấy giá trị từ session (chuyển byte[] thành đối tượng)
        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionFactory.TryGetValue(key, out value);
        }

        // Lấy đối tượng từ session (chuyển byte[] thành đối tượng thực tế)
        public T Get<T>(string key)
        {
            if (_sessionFactory.TryGetValue(key, out var value))
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        // Lưu đối tượng vào session (chuyển đối tượng thành byte[])
        public void Set<T>(string key, T value)
        {
            var serializedValue = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value);
            _sessionFactory[key] = serializedValue;
        }
    }
}
