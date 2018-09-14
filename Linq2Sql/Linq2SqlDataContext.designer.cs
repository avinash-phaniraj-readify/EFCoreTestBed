﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Linq2SqlEFCoreBehaviorsTest.Linq2Sql
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FruitCake")]
	public partial class Linq2SqlDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertEmployee(Employee instance);
    partial void UpdateEmployee(Employee instance);
    partial void DeleteEmployee(Employee instance);
    partial void InsertEmployeeDevice(EmployeeDevice instance);
    partial void UpdateEmployeeDevice(EmployeeDevice instance);
    partial void DeleteEmployeeDevice(EmployeeDevice instance);
    partial void InsertEmployeeDetail(EmployeeDetail instance);
    partial void UpdateEmployeeDetail(EmployeeDetail instance);
    partial void DeleteEmployeeDetail(EmployeeDetail instance);
    #endregion
		
		public Linq2SqlDataContext() : 
				base(global::Linq2SqlEFCoreBehaviorsTest.Properties.Settings.Default.FruitCakeConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public Linq2SqlDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Linq2SqlDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Linq2SqlDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Linq2SqlDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Employee> Employees
		{
			get
			{
				return this.GetTable<Employee>();
			}
		}
		
		public System.Data.Linq.Table<EmployeeDevice> EmployeeDevices
		{
			get
			{
				return this.GetTable<EmployeeDevice>();
			}
		}
		
		public System.Data.Linq.Table<EmployeeDetail> EmployeeDetails
		{
			get
			{
				return this.GetTable<EmployeeDetail>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="Employee")]
	public partial class Employee : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private EntitySet<EmployeeDevice> _EmployeeDevices;
		
		private EntityRef<EmployeeDetail> _EmployeeDetails;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public Employee()
		{
			this._EmployeeDevices = new EntitySet<EmployeeDevice>(new Action<EmployeeDevice>(this.attach_EmployeeDevices), new Action<EmployeeDevice>(this.detach_EmployeeDevices));
			this._EmployeeDetails = default(EntityRef<EmployeeDetail>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Employee_EmployeeDevice", Storage="_EmployeeDevices", ThisKey="Id", OtherKey="EmployeeId")]
		public EntitySet<EmployeeDevice> EmployeeDevices
		{
			get
			{
				return this._EmployeeDevices;
			}
			set
			{
				this._EmployeeDevices.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Employee_EmployeeDetail", Storage="_EmployeeDetails", ThisKey="Id", OtherKey="EmployeeId", IsUnique=true, IsForeignKey=false)]
		public EmployeeDetail EmployeeDetails
		{
			get
			{
				return this._EmployeeDetails.Entity;
			}
			set
			{
				EmployeeDetail previousValue = this._EmployeeDetails.Entity;
				if (((previousValue != value) 
							|| (this._EmployeeDetails.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._EmployeeDetails.Entity = null;
						previousValue.Employee = null;
					}
					this._EmployeeDetails.Entity = value;
					if ((value != null))
					{
						value.Employee = this;
					}
					this.SendPropertyChanged("EmployeeDetails");
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
		
		private void attach_EmployeeDevices(EmployeeDevice entity)
		{
			this.SendPropertyChanging();
			entity.Employee = this;
		}
		
		private void detach_EmployeeDevices(EmployeeDevice entity)
		{
			this.SendPropertyChanging();
			entity.Employee = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="EmployeeDevice")]
	public partial class EmployeeDevice : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _EmployeeId;
		
		private string _Device;
		
		private EntityRef<Employee> _Employee;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnEmployeeIdChanging(int value);
    partial void OnEmployeeIdChanged();
    partial void OnDeviceChanging(string value);
    partial void OnDeviceChanged();
    #endregion
		
		public EmployeeDevice()
		{
			this._Employee = default(EntityRef<Employee>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeId", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		public int EmployeeId
		{
			get
			{
				return this._EmployeeId;
			}
			set
			{
				if ((this._EmployeeId != value))
				{
					if (this._Employee.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnEmployeeIdChanging(value);
					this.SendPropertyChanging();
					this._EmployeeId = value;
					this.SendPropertyChanged("EmployeeId");
					this.OnEmployeeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Device", DbType="NVarChar(10) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Device
		{
			get
			{
				return this._Device;
			}
			set
			{
				if ((this._Device != value))
				{
					this.OnDeviceChanging(value);
					this.SendPropertyChanging();
					this._Device = value;
					this.SendPropertyChanged("Device");
					this.OnDeviceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Employee_EmployeeDevice", Storage="_Employee", ThisKey="EmployeeId", OtherKey="Id", IsForeignKey=true)]
		public Employee Employee
		{
			get
			{
				return this._Employee.Entity;
			}
			set
			{
				Employee previousValue = this._Employee.Entity;
				if (((previousValue != value) 
							|| (this._Employee.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Employee.Entity = null;
						previousValue.EmployeeDevices.Remove(this);
					}
					this._Employee.Entity = value;
					if ((value != null))
					{
						value.EmployeeDevices.Add(this);
						this._EmployeeId = value.Id;
					}
					else
					{
						this._EmployeeId = default(int);
					}
					this.SendPropertyChanged("Employee");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="EmployeeDetails")]
	public partial class EmployeeDetail : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _EmployeeId;
		
		private string _Details;
		
		private EntityRef<Employee> _Employee;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnEmployeeIdChanging(int value);
    partial void OnEmployeeIdChanged();
    partial void OnDetailsChanging(string value);
    partial void OnDetailsChanged();
    #endregion
		
		public EmployeeDetail()
		{
			this._Employee = default(EntityRef<Employee>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeId", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		public int EmployeeId
		{
			get
			{
				return this._EmployeeId;
			}
			set
			{
				if ((this._EmployeeId != value))
				{
					if (this._Employee.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnEmployeeIdChanging(value);
					this.SendPropertyChanging();
					this._EmployeeId = value;
					this.SendPropertyChanged("EmployeeId");
					this.OnEmployeeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Details", DbType="NVarChar(100) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Details
		{
			get
			{
				return this._Details;
			}
			set
			{
				if ((this._Details != value))
				{
					this.OnDetailsChanging(value);
					this.SendPropertyChanging();
					this._Details = value;
					this.SendPropertyChanged("Details");
					this.OnDetailsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Employee_EmployeeDetail", Storage="_Employee", ThisKey="EmployeeId", OtherKey="Id", IsForeignKey=true)]
		public Employee Employee
		{
			get
			{
				return this._Employee.Entity;
			}
			set
			{
				Employee previousValue = this._Employee.Entity;
				if (((previousValue != value) 
							|| (this._Employee.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Employee.Entity = null;
						previousValue.EmployeeDetails = null;
					}
					this._Employee.Entity = value;
					if ((value != null))
					{
						value.EmployeeDetails = this;
						this._EmployeeId = value.Id;
					}
					else
					{
						this._EmployeeId = default(int);
					}
					this.SendPropertyChanged("Employee");
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
}
#pragma warning restore 1591
