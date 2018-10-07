//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Linq.Mapping;
//using System.Linq;
//using System.Web;

//namespace dip.Models.Domain
//{

   
//    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Actions")]
//    public partial class Action : INotifyPropertyChanging, INotifyPropertyChanged
//    {

//        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

//        private int _id;

//        private string _actionId;

//        private string _actionType;

//        private string _fizVelId;

//        #region Extensibility Method Definitions
//        partial void OnLoaded();
//        partial void OnValidate(System.Data.Linq.ChangeAction action);
//        partial void OnCreated();
//        partial void OnidChanging(int value);
//        partial void OnidChanged();
//        partial void OnactionIdChanging(string value);
//        partial void OnactionIdChanged();
//        partial void OnactionTypeChanging(string value);
//        partial void OnactionTypeChanged();
//        partial void OnfizVelIdChanging(string value);
//        partial void OnfizVelIdChanged();
//        #endregion

//        public Action()
//        {
//            OnCreated();
//        }

//        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
//        public int id
//        {
//            get
//            {
//                return this._id;
//            }
//            set
//            {
//                if ((this._id != value))
//                {
//                    this.OnidChanging(value);
//                    this.SendPropertyChanging();
//                    this._id = value;
//                    this.SendPropertyChanged("id");
//                    this.OnidChanged();
//                }
//            }
//        }

//        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_actionId", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
//        public string actionId
//        {
//            get
//            {
//                return this._actionId;
//            }
//            set
//            {
//                if ((this._actionId != value))
//                {
//                    this.OnactionIdChanging(value);
//                    this.SendPropertyChanging();
//                    this._actionId = value;
//                    this.SendPropertyChanged("actionId");
//                    this.OnactionIdChanged();
//                }
//            }
//        }

//        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_actionType", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
//        public string actionType
//        {
//            get
//            {
//                return this._actionType;
//            }
//            set
//            {
//                if ((this._actionType != value))
//                {
//                    this.OnactionTypeChanging(value);
//                    this.SendPropertyChanging();
//                    this._actionType = value;
//                    this.SendPropertyChanged("actionType");
//                    this.OnactionTypeChanged();
//                }
//            }
//        }

//        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_fizVelId", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
//        public string fizVelId
//        {
//            get
//            {
//                return this._fizVelId;
//            }
//            set
//            {
//                if ((this._fizVelId != value))
//                {
//                    this.OnfizVelIdChanging(value);
//                    this.SendPropertyChanging();
//                    this._fizVelId = value;
//                    this.SendPropertyChanged("fizVelId");
//                    this.OnfizVelIdChanged();
//                }
//            }
//        }

//        public event PropertyChangingEventHandler PropertyChanging;

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void SendPropertyChanging()
//        {
//            if ((this.PropertyChanging != null))
//            {
//                this.PropertyChanging(this, emptyChangingEventArgs);
//            }
//        }

//        protected virtual void SendPropertyChanged(String propertyName)
//        {
//            if ((this.PropertyChanged != null))
//            {
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }













//}

