﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIXVideos.Oxite.Repositories
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="Oxite.MIXVideos")]
	public partial class OxitePostViewDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Insertoxite_PostView(oxite_PostView instance);
    partial void Updateoxite_PostView(oxite_PostView instance);
    partial void Deleteoxite_PostView(oxite_PostView instance);
    partial void Insertoxite_PostViewType(oxite_PostViewType instance);
    partial void Updateoxite_PostViewType(oxite_PostViewType instance);
    partial void Deleteoxite_PostViewType(oxite_PostViewType instance);
    #endregion
		
		public OxitePostViewDataContext() : 
				base("Data Source=.\\SQLEXPRESS;Initial Catalog=Oxite.MIXVideos;Integrated Security=True" +
						"", mappingSource)
		{
			OnCreated();
		}
		
		public OxitePostViewDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OxitePostViewDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OxitePostViewDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OxitePostViewDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<oxite_PostView> oxite_PostViews
		{
			get
			{
				return this.GetTable<oxite_PostView>();
			}
		}
		
		public System.Data.Linq.Table<oxite_PostViewType> oxite_PostViewTypes
		{
			get
			{
				return this.GetTable<oxite_PostViewType>();
			}
		}
	}
	
	[Table(Name="dbo.oxite_PostView")]
	public partial class oxite_PostView : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _PostID;
		
		private System.Guid _PostViewTypeID;
		
		private System.Guid _PostViewID;
		
		private int _PostViewCount;
		
		private System.DateTime _PostViewDate;
		
		private EntityRef<oxite_PostViewType> _oxite_PostViewType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPostIDChanging(System.Guid value);
    partial void OnPostIDChanged();
    partial void OnPostViewTypeIDChanging(System.Guid value);
    partial void OnPostViewTypeIDChanged();
    partial void OnPostViewIDChanging(System.Guid value);
    partial void OnPostViewIDChanged();
    partial void OnPostViewCountChanging(int value);
    partial void OnPostViewCountChanged();
    partial void OnPostViewDateChanging(System.DateTime value);
    partial void OnPostViewDateChanged();
    #endregion
		
		public oxite_PostView()
		{
			this._oxite_PostViewType = default(EntityRef<oxite_PostViewType>);
			OnCreated();
		}
		
		[Column(Storage="_PostID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid PostID
		{
			get
			{
				return this._PostID;
			}
			set
			{
				if ((this._PostID != value))
				{
					this.OnPostIDChanging(value);
					this.SendPropertyChanging();
					this._PostID = value;
					this.SendPropertyChanged("PostID");
					this.OnPostIDChanged();
				}
			}
		}
		
		[Column(Storage="_PostViewTypeID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid PostViewTypeID
		{
			get
			{
				return this._PostViewTypeID;
			}
			set
			{
				if ((this._PostViewTypeID != value))
				{
					if (this._oxite_PostViewType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnPostViewTypeIDChanging(value);
					this.SendPropertyChanging();
					this._PostViewTypeID = value;
					this.SendPropertyChanged("PostViewTypeID");
					this.OnPostViewTypeIDChanged();
				}
			}
		}
		
		[Column(Storage="_PostViewID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid PostViewID
		{
			get
			{
				return this._PostViewID;
			}
			set
			{
				if ((this._PostViewID != value))
				{
					this.OnPostViewIDChanging(value);
					this.SendPropertyChanging();
					this._PostViewID = value;
					this.SendPropertyChanged("PostViewID");
					this.OnPostViewIDChanged();
				}
			}
		}
		
		[Column(Storage="_PostViewCount", DbType="Int NOT NULL")]
		public int PostViewCount
		{
			get
			{
				return this._PostViewCount;
			}
			set
			{
				if ((this._PostViewCount != value))
				{
					this.OnPostViewCountChanging(value);
					this.SendPropertyChanging();
					this._PostViewCount = value;
					this.SendPropertyChanged("PostViewCount");
					this.OnPostViewCountChanged();
				}
			}
		}
		
		[Column(Storage="_PostViewDate", DbType="DateTime NOT NULL")]
		public System.DateTime PostViewDate
		{
			get
			{
				return this._PostViewDate;
			}
			set
			{
				if ((this._PostViewDate != value))
				{
					this.OnPostViewDateChanging(value);
					this.SendPropertyChanging();
					this._PostViewDate = value;
					this.SendPropertyChanged("PostViewDate");
					this.OnPostViewDateChanged();
				}
			}
		}
		
		[Association(Name="oxite_PostViewType_oxite_PostView", Storage="_oxite_PostViewType", ThisKey="PostViewTypeID", OtherKey="PostViewTypeID", IsForeignKey=true)]
		public oxite_PostViewType oxite_PostViewType
		{
			get
			{
				return this._oxite_PostViewType.Entity;
			}
			set
			{
				oxite_PostViewType previousValue = this._oxite_PostViewType.Entity;
				if (((previousValue != value) 
							|| (this._oxite_PostViewType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._oxite_PostViewType.Entity = null;
						previousValue.oxite_PostViews.Remove(this);
					}
					this._oxite_PostViewType.Entity = value;
					if ((value != null))
					{
						value.oxite_PostViews.Add(this);
						this._PostViewTypeID = value.PostViewTypeID;
					}
					else
					{
						this._PostViewTypeID = default(System.Guid);
					}
					this.SendPropertyChanged("oxite_PostViewType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.oxite_PostViewType")]
	public partial class oxite_PostViewType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _PostViewTypeID;
		
		private string _PostViewTypeName;
		
		private EntitySet<oxite_PostView> _oxite_PostViews;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPostViewTypeIDChanging(System.Guid value);
    partial void OnPostViewTypeIDChanged();
    partial void OnPostViewTypeNameChanging(string value);
    partial void OnPostViewTypeNameChanged();
    #endregion
		
		public oxite_PostViewType()
		{
			this._oxite_PostViews = new EntitySet<oxite_PostView>(new Action<oxite_PostView>(this.attach_oxite_PostViews), new Action<oxite_PostView>(this.detach_oxite_PostViews));
			OnCreated();
		}
		
		[Column(Storage="_PostViewTypeID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid PostViewTypeID
		{
			get
			{
				return this._PostViewTypeID;
			}
			set
			{
				if ((this._PostViewTypeID != value))
				{
					this.OnPostViewTypeIDChanging(value);
					this.SendPropertyChanging();
					this._PostViewTypeID = value;
					this.SendPropertyChanged("PostViewTypeID");
					this.OnPostViewTypeIDChanged();
				}
			}
		}
		
		[Column(Storage="_PostViewTypeName", DbType="NVarChar(25) NOT NULL", CanBeNull=false)]
		public string PostViewTypeName
		{
			get
			{
				return this._PostViewTypeName;
			}
			set
			{
				if ((this._PostViewTypeName != value))
				{
					this.OnPostViewTypeNameChanging(value);
					this.SendPropertyChanging();
					this._PostViewTypeName = value;
					this.SendPropertyChanged("PostViewTypeName");
					this.OnPostViewTypeNameChanged();
				}
			}
		}
		
		[Association(Name="oxite_PostViewType_oxite_PostView", Storage="_oxite_PostViews", ThisKey="PostViewTypeID", OtherKey="PostViewTypeID")]
		public EntitySet<oxite_PostView> oxite_PostViews
		{
			get
			{
				return this._oxite_PostViews;
			}
			set
			{
				this._oxite_PostViews.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_oxite_PostViews(oxite_PostView entity)
		{
			this.SendPropertyChanging();
			entity.oxite_PostViewType = this;
		}
		
		private void detach_oxite_PostViews(oxite_PostView entity)
		{
			this.SendPropertyChanging();
			entity.oxite_PostViewType = null;
		}
	}
}
#pragma warning restore 1591
