Mini2.define("Mini2.data.ResultSet",{loaded:true,count:0,total:0,success:false,constructor:function(a){Mini2.apply(this,a);this.totalRecords=this.total;if(a.count===undefined){this.count=this.records.length}}});