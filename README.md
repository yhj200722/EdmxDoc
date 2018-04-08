# EdmxDoc
add documentation summary for entity framework 6.x edmx file from database table and column comment, now for sqlserver only, you can add other database implementation.

为entity framework edmx文件添加数据库表及字段的注释，再根据edmx文件生成带注释的实体及上下文类，目前只针对sqlserver，用的ef6.x，你要用其它数据库的话，自己添加其它数据库的注释生成代码

use steps:

 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/1.png) 
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/2.png) 
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/3.png) 
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/4.png) 
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/5.png)
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/6.png)
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/7.png)
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/8.png)

copy 'Db.Context.tt' and 'Db.tt' files at EdmxDoc\T4 directory to your project directory where your edmx file at, replace your 'xxx.Context.tt' and 'xxx.Db.tt' files, then open the two .tt files, modify database connection string to meet your requirement, save it, that's all.

将Edmx项目T4目录下的2个.tt文件复制到你的edmx文件所在目录，替换原有的2个.tt文件，然后打开.tt文件，将数据库连接字符串改为你自己的，保存即可

 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/11.png)

最终效果

 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/9.png)
 ![image](https://github.com/yhj200722/EdmxDoc/raw/master/screenshots/10.png)
