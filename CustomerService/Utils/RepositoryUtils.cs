using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Utils {
    public static class RepositoryUtils {

        public static bool isQueryValid<T>(IQueryCollection query) {
            foreach (var item in query) {
                var val = typeof(T).GetProperty(item.Key);
                // key exists check
                if (val == null) {
                    return false;
                }
                // empty value check
                if (item.Value == "") {
                    return false;
                }
            }
            return true;
        }

        public static bool isObjectEmpty<T>(T obj) {
            foreach (PropertyInfo prop in typeof(T).GetProperties()) {
                if (prop.GetValue(obj, null) == null) {
                    return true;
                }
            }
            return false;
        }

        public static string getVal<T>(T obj, string val) {
            return obj.GetType().GetProperty(val).GetValue(obj, null).ToString();
        }

    }

    public class NotValidQueryException : Exception {
        public NotValidQueryException(string message) : base(message) { }
    }

}
