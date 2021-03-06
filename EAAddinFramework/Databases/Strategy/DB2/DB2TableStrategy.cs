﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace EAAddinFramework.Databases.Strategy.DB2
{
	/// <summary>
	/// Description of DB2TableStrategy.
	/// </summary>
	public class DB2TableStrategy:DB2Strategy
	{
		View linkedView {get;set;}
		bool isNew = false;
		Table table 
		{
			get
			{
				return (Table)this.databaseItem;
			}
		}
		public DB2TableStrategy(StrategyFactory factory):base(factory) {}

		public override void beforeSave()
		{
			//new table
			if (databaseItem.wrappedElement == null)
			{
				this.isNew = true;
				//create new view
				linkedView = new View((Database)table.databaseOwner,this.getViewName() ,factory.getStrategy<View>());
				this.linkedView.definition = getViewDefinition();
			}
			else
			{
				this.isNew = false;
			}
		}
		public override void afterSave()
		{
			if (isNew)
			{
				if (this.linkedView != null)
				{
					this.linkedView.save();
				}
			}
		}
		private string getViewName()
		{
			var nameStringBuilder = new StringBuilder(this.table.name);
			nameStringBuilder[2] = 'V';
			return nameStringBuilder.ToString();
		}
		private string getViewDefinition()
		{
			return "select * from " + table.tableOwner + "." + this.table.name;
		}

	}
}
