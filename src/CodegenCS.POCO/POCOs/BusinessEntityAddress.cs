﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("BusinessEntityAddress", Schema = "Person")]
    public partial class BusinessEntityAddress : INotifyPropertyChanged
    {
        #region Members
        private int _businessEntityId;
        [Key]
        public int BusinessEntityId 
        { 
            get { return _businessEntityId; } 
            set { SetField(ref _businessEntityId, value, nameof(BusinessEntityId)); } 
        }
        private int _addressId;
        [Key]
        public int AddressId 
        { 
            get { return _addressId; } 
            set { SetField(ref _addressId, value, nameof(AddressId)); } 
        }
        private int _addressTypeId;
        [Key]
        public int AddressTypeId 
        { 
            get { return _addressTypeId; } 
            set { SetField(ref _addressTypeId, value, nameof(AddressTypeId)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private Guid _rowguid;
        public Guid Rowguid 
        { 
            get { return _rowguid; } 
            set { SetField(ref _rowguid, value, nameof(Rowguid)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (BusinessEntityId == default(int) && AddressId == default(int) && AddressTypeId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Person].[BusinessEntityAddress]
                (
                    [AddressID],
                    [AddressTypeID],
                    [BusinessEntityID],
                    [ModifiedDate]
                )
                VALUES
                (
                    @AddressId,
                    @AddressTypeId,
                    @BusinessEntityId,
                    @ModifiedDate
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Person].[BusinessEntityAddress] SET
                    [AddressID] = @AddressId,
                    [AddressTypeID] = @AddressTypeId,
                    [BusinessEntityID] = @BusinessEntityId,
                    [ModifiedDate] = @ModifiedDate
                WHERE
                    [BusinessEntityID] = @BusinessEntityId AND 
                    [AddressID] = @AddressId AND 
                    [AddressTypeID] = @AddressTypeId";
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
            BusinessEntityAddress other = obj as BusinessEntityAddress;
            if (other == null) return false;

            if (AddressId != other.AddressId)
                return false;
            if (AddressTypeId != other.AddressTypeId)
                return false;
            if (BusinessEntityId != other.BusinessEntityId)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Rowguid != other.Rowguid)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (AddressId == default(int) ? 0 : AddressId.GetHashCode());
                hash = hash * 23 + (AddressTypeId == default(int) ? 0 : AddressTypeId.GetHashCode());
                hash = hash * 23 + (BusinessEntityId == default(int) ? 0 : BusinessEntityId.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Rowguid == default(Guid) ? 0 : Rowguid.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(BusinessEntityAddress left, BusinessEntityAddress right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BusinessEntityAddress left, BusinessEntityAddress right)
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
