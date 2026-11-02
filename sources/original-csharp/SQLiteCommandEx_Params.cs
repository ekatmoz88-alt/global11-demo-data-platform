using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace DbStationary.Data
{
    public partial class SQLiteCommandEx<T>
    {
        public DbParameter CreateParam(string name, DbType dataType, object value)
        {
            var param = new SQLiteParameter(name, dataType);
            param.Value = SetParamValue(value);
            _command.Parameters.Add(param);
            return param;
        }

        public DbParameter CreateParam(string name, DbType dataType)
        {
            var param = new SQLiteParameter(name, dataType);
            _command.Parameters.Add(param);
            return param;
        }

        public SQLiteCommandEx<T> Add<TV>(string name, TV value)
        {
            var param = new SQLiteParameter(name, value);
            _command.Parameters.Add(param);
            return this;
        }

        public SQLiteCommandEx<T> Add(string name)
        {
            var param = new SQLiteParameter {ParameterName = name};
            _command.Parameters.Add(param);
            return this;
        }

        public SQLiteCommandEx<T> AddInt(string name)
        {
            CreateParam(name, DbType.Int32);
            return this;
        }

        public SQLiteCommandEx<T> AddInt(string name, int value)
        {
            CreateParam(name, DbType.Int32, value);
            return this;
        }

        public SQLiteCommandEx<T> AddInt(string name, int? value)
        {
            CreateParam(name, DbType.Int32, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDecimal(string name)
        {
            CreateParam(name, DbType.Decimal);
            return this;
        }
        public SQLiteCommandEx<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, DbType.Decimal, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, DbType.Decimal, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTime, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTime(string name)
        {
            CreateParam(name, DbType.DateTime);
            return this;
        }
        public SQLiteCommandEx<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTime, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTime2(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTime2, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTime2(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTime2, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTimeOffset(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTimeOffset, value);
            return this;
        }

        public SQLiteCommandEx<T> AddDateTimeOffset(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTimeOffset, value);
            return this;
        }

        public SQLiteCommandEx<T> AddString(string name, int size)
        {
            var param = CreateParam(name, DbType.String);
            param.Size = size;
            return this;
        }

        public SQLiteCommandEx<T> AddString(string name, int size, string value)
        {
            var param = CreateParam(name, DbType.String, value);
            param.Size = size;
            return this;
        }

        public SQLiteCommandEx<T> AddString(string name, string value)
        {
            CreateParam(name, DbType.String, value);
            return this;
        }
        public SQLiteCommandEx<T> AddString(string name)
        {
            CreateParam(name, DbType.String);
            return this;
        }

        public SQLiteCommandEx<T> AddGuid(string name, Guid value)
        {
            CreateParam(name, DbType.Guid, value);
            return this;
        }
        public SQLiteCommandEx<T> AddGuid(string name)
        {
            CreateParam(name, DbType.Guid);
            return this;
        }

        public SQLiteCommandEx<T> AddGuidID(string name)
        {
            CreateParam(name, DbType.Guid, Guid.NewGuid());
            return this;
        }

        public SQLiteCommandEx<T> AddBoolean(string name)
        {
            CreateParam(name, DbType.Boolean);
            return this;
        }
        public SQLiteCommandEx<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, DbType.Boolean, value);
            return this;
        }
    }
}
