﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("ProductPhoto", Schema = "Production")]
    public partial class ProductPhoto : INotifyPropertyChanged
    {
        #region Members
        private int _productPhotoId;
        [Key]
        public int ProductPhotoId 
        { 
            get { return _productPhotoId; } 
            set { SetField(ref _productPhotoId, value, nameof(ProductPhotoId)); } 
        }
        private Byte[] _largePhoto;
        public Byte[] LargePhoto 
        { 
            get { return _largePhoto; } 
            set { SetField(ref _largePhoto, value, nameof(LargePhoto)); } 
        }
        private string _largePhotoFileName;
        public string LargePhotoFileName 
        { 
            get { return _largePhotoFileName; } 
            set { SetField(ref _largePhotoFileName, value, nameof(LargePhotoFileName)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private Byte[] _thumbNailPhoto;
        public Byte[] ThumbNailPhoto 
        { 
            get { return _thumbNailPhoto; } 
            set { SetField(ref _thumbNailPhoto, value, nameof(ThumbNailPhoto)); } 
        }
        private string _thumbnailPhotoFileName;
        public string ThumbnailPhotoFileName 
        { 
            get { return _thumbnailPhotoFileName; } 
            set { SetField(ref _thumbnailPhotoFileName, value, nameof(ThumbnailPhotoFileName)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (ProductPhotoId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Production].[ProductPhoto]
                (
                    [LargePhoto],
                    [LargePhotoFileName],
                    [ModifiedDate],
                    [ThumbNailPhoto],
                    [ThumbnailPhotoFileName]
                )
                VALUES
                (
                    @LargePhoto,
                    @LargePhotoFileName,
                    @ModifiedDate,
                    @ThumbNailPhoto,
                    @ThumbnailPhotoFileName
                )";

                this.ProductPhotoId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Production].[ProductPhoto] SET
                    [LargePhoto] = @LargePhoto,
                    [LargePhotoFileName] = @LargePhotoFileName,
                    [ModifiedDate] = @ModifiedDate,
                    [ThumbNailPhoto] = @ThumbNailPhoto,
                    [ThumbnailPhotoFileName] = @ThumbnailPhotoFileName
                WHERE
                    [ProductPhotoID] = @ProductPhotoId";
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
            ProductPhoto other = obj as ProductPhoto;
            if (other == null) return false;

            if (LargePhoto != other.LargePhoto)
                return false;
            if (LargePhotoFileName != other.LargePhotoFileName)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (ThumbNailPhoto != other.ThumbNailPhoto)
                return false;
            if (ThumbnailPhotoFileName != other.ThumbnailPhotoFileName)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (LargePhoto == null ? 0 : LargePhoto.GetHashCode());
                hash = hash * 23 + (LargePhotoFileName == null ? 0 : LargePhotoFileName.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (ThumbNailPhoto == null ? 0 : ThumbNailPhoto.GetHashCode());
                hash = hash * 23 + (ThumbnailPhotoFileName == null ? 0 : ThumbnailPhotoFileName.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(ProductPhoto left, ProductPhoto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductPhoto left, ProductPhoto right)
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
