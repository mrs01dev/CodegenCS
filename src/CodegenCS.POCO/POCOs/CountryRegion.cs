﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("CountryRegion", Schema = "Person")]
    public partial class CountryRegion : INotifyPropertyChanged
    {
        #region Members
        private string _countryRegionCode;
        [Key]
        public string CountryRegionCode 
        { 
            get { return _countryRegionCode; } 
            set { SetField(ref _countryRegionCode, value, nameof(CountryRegionCode)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { SetField(ref _name, value, nameof(Name)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (CountryRegionCode == null)
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Person].[CountryRegion]
                (
                    [CountryRegionCode],
                    [ModifiedDate],
                    [Name]
                )
                VALUES
                (
                    @CountryRegionCode,
                    @ModifiedDate,
                    @Name
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Person].[CountryRegion] SET
                    [CountryRegionCode] = @CountryRegionCode,
                    [ModifiedDate] = @ModifiedDate,
                    [Name] = @Name
                WHERE
                    [CountryRegionCode] = @CountryRegionCode";
                conn.Execute(cmd, this);
            }
        }
        #endregion ActiveRecord

        #region Equals/GetHashCode
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            CountryRegion other = obj as CountryRegion;
            if (other == null) return false;

            if (CountryRegionCode != other.CountryRegionCode)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Name != other.Name)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (CountryRegionCode == null ? 0 : CountryRegionCode.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(CountryRegion left, CountryRegion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CountryRegion left, CountryRegion right)
        {
            return !Equals(left, right);
        }

        #endregion Equals/GetHashCode

        #region INotifyPropertyChanged/IsDirty
        public HashSet<string> ChangedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public void MarkAsClean()
        {
            ChangedProperties.Clear();
        }
        public virtual bool IsDirty => ChangedProperties.Any();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetField<T>(ref T field, T value, string propertyName) {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                ChangedProperties.Add(propertyName);
                OnPropertyChanged(propertyName);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged/IsDirty
    }
}
