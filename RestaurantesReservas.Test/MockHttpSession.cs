using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantesReservas.Test
{
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public bool IsAvailable => true;

        public string Id => "mock_session_id";

        public void Clear() => _sessionStorage.Clear();

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void SetString(string key, string value)
        {
            Set(key, Encoding.UTF8.GetBytes(value));
        }

        public string GetString(string key)
        {
            return TryGetValue(key, out var data) ? Encoding.UTF8.GetString(data) : null;
        }
    }
}
