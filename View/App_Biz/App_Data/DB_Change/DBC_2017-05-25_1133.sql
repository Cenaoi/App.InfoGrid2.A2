--2017-5-25

--对表 [FLOW_INST] 增加字段

alter table [FLOW_INST] add [EXT_COL_VALUE_1] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_VALUE_2] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_VALUE_3] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_VALUE_4] nvarchar(50) default '' NOT NULL 

alter table [FLOW_INST] add [EXT_COL_TEXT_1] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_TEXT_2] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_TEXT_3] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_TEXT_4] nvarchar(20) default '' NOT NULL 

alter table [FLOW_INST] add [EXT_COL_1] varchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_2] varchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_3] varchar(50) default '' NOT NULL 
alter table [FLOW_INST] add [EXT_COL_4] varchar(50) default '' NOT NULL 



--对表 [FLOW_INST_COPY] 增加字段

alter table [FLOW_INST_COPY] add [EXT_COL_VALUE_1] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_VALUE_2] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_VALUE_3] nvarchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_VALUE_4] nvarchar(50) default '' NOT NULL 

alter table [FLOW_INST_COPY] add [EXT_COL_TEXT_1] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_TEXT_2] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_TEXT_3] nvarchar(20) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_TEXT_4] nvarchar(20) default '' NOT NULL 

alter table [FLOW_INST_COPY] add [EXT_COL_1] varchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_2] varchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_3] varchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [EXT_COL_4] varchar(50) default '' NOT NULL 

--抄送表, 增加字段 2017-6-6
alter table [FLOW_INST_COPY] add [START_USER_CODE] varchar(50) default '' NOT NULL 
alter table [FLOW_INST_COPY] add [START_USER_TEXT] varchar(50) default '' NOT NULL 