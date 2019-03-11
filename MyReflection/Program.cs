using Ruanmou.DB.Interface;
using Ruanmou.DB.MySql;
using Ruanmou.DB.SqlServer;
using Ruanmou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyReflection
{
    /// <summary>
    /// 1 dll-IL-metadata-反射
    /// 2 反射加载dll，读取module、类、方法、特性
    /// 3 反射创建对象，反射+简单工厂+配置文件  选修 ：多参数构造函数 破坏单例 创建泛型
    /// 4 反射调用实例方法、静态方法、重载方法 选修:调用私有方法 调用泛型方法
    /// 5 反射字段和属性，分别获取值和设置值
    /// 6 反射的好处和局限
    /// 
    /// 反射：是.Net Framework提供的一个帮助类库，可以访问dll的metadata，并且使用它
    /// 
    /// 反射的优缺点：
    /// 动态
    /// 
    /// 1 代码多 写起来烦
    /// 2 避开编译器的检查
    /// 3 性能问题 
    ///       反射性能要差普通的400+倍
    ///       但是绝对值小，几乎不影响项目性能
    ///       而且还可以优化，空间换时间(非常适合泛型缓存)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("欢迎来到.net高级班vip课程，今天是Eleven老师为大家带来的反射的课程");
                #region Common
                {
                    Console.WriteLine("************************Common*****************");
                    IDBHelper iDBHelper = new MySqlHelper();
                    iDBHelper.Query();
                }
                #endregion

                #region Reflection
                //{
                //    Console.WriteLine("************************Reflection*****************");
                //    Assembly assemly = Assembly.Load("Ruanmou.DB.MySql");//加载dll路径的名称的dll/exe  不需要后缀
                //    #region 
                //    //Assembly assemly1 = Assembly.LoadFile(@"D:\ruanmou\online10\20171101Advanced10Course2Reflection\MyReflection\MyReflection\bin\Debug\Ruanmou.DB.Interface.dll");
                //    //Assembly assemly2 = Assembly.LoadFrom("Ruanmou.DB.MySql.dll");
                //    //只要dll 在不会异常，使用的时候，没有依赖项，可能异常
                //    //foreach (var item in assemly.GetModules())
                //    //{
                //    //    Console.WriteLine(item.Name);
                //    //}
                //    //foreach (var type in assemly.GetTypes())
                //    //{
                //    //    Console.WriteLine(type.Name);
                //    //    foreach (var item in type.GetMethods())
                //    //    {
                //    //        Console.WriteLine(item.Name);
                //    //    }
                //    //}
                //    #endregion

                //    Type typeDBHelper = assemly.GetType("Ruanmou.DB.MySql.MySqlHelper");//2 获取类型信息
                //    object oDBHelper = Activator.CreateInstance(typeDBHelper);//3 创建对象
                //    //oDBHelper.Query();
                //    IDBHelper iDBHelper = oDBHelper as IDBHelper;//4 类型转换
                //    iDBHelper.Query();//5 方法调用
                //}
                #endregion

                #region Reflection+Factory+Config   IOC的雏形
                //{
                //    Console.WriteLine("************************Reflection+Factory+Config*****************");
                //    IDBHelper iDBHelper = SimpleFactory.CreateHelper();
                //    iDBHelper.Query();
                //}
                #endregion

                #region Reflection 选修
                {
                    //Console.WriteLine("************************Reflection*****************");
                    Assembly assemly = Assembly.Load("Ruanmou.DB.SqlServer");

                    Type testType = assemly.GetType("Ruanmou.DB.SqlServer.ReflectionTest");
                    object oTest1 = Activator.CreateInstance(testType);
                    object oTest2 = Activator.CreateInstance(testType, new object[] { });
                    object oTest3 = Activator.CreateInstance(testType, new object[] { 895 });
                    object oTest4 = Activator.CreateInstance(testType, new object[] { "桃树" });

                    Type genericType = assemly.GetType("Ruanmou.DB.SqlServer.GenericClass`3");
                    Type genericNewType = genericType.MakeGenericType(typeof(int), typeof(string), typeof(Program));
                    object oGeneric = Activator.CreateInstance(genericNewType);

                    Type singletonType = assemly.GetType("Ruanmou.DB.SqlServer.Singleton");
                    //Singleton singleton = new Singleton();
                    object oSingleton1 = Activator.CreateInstance(singletonType, true);
                    object oSingleton2 = Activator.CreateInstance(singletonType, true);
                    object oSingleton3 = Activator.CreateInstance(singletonType, true);
                }
                #endregion

                #region Reflection Method  MVC
                {
                    //Console.WriteLine("************************Reflection*****************");
                    Assembly assemly = Assembly.Load("Ruanmou.DB.SqlServer");
                    Type testType = assemly.GetType("Ruanmou.DB.SqlServer.ReflectionTest");
                    object oTest = Activator.CreateInstance(testType);
                    {
                        MethodInfo method = testType.GetMethod("Show1");//以前写的具体方法，换成字符串
                        method.Invoke(oTest, null);
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show2");//带参数
                        method.Invoke(oTest, new object[] { 1332 });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show5");//静态
                        method.Invoke(oTest, new object[] { "修罗" });
                        method.Invoke(null, new object[] { "修罗" });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show3", new Type[] { });//重载方法
                        method.Invoke(oTest, new object[] { });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show3", new Type[] { typeof(int) });//重载方法
                        method.Invoke(oTest, new object[] { 1406 });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show3", new Type[] { typeof(string) });//重载方法
                        method.Invoke(oTest, new object[] { "晴空" });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show3", new Type[] { typeof(int), typeof(string) });//重载方法
                        method.Invoke(oTest, new object[] { 1214, "小康" });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show3", new Type[] { typeof(string), typeof(int) });//重载方法
                        method.Invoke(oTest, new object[] { "一个人的孤单", 1284 });
                    }
                    {
                        MethodInfo method = testType.GetMethod("Show4", BindingFlags.Instance | BindingFlags.NonPublic);//私有方法
                        method.Invoke(oTest, new object[] { "若然" });
                    }
                    {
                        Type genericType = assemly.GetType("Ruanmou.DB.SqlServer.GenericMethod");
                        object oGeneric = Activator.CreateInstance(genericType);
                        MethodInfo method = genericType.GetMethod("Show");
                        //MethodInfo method = genericType.GetMethod("Show`3");
                        MethodInfo methodNew = method.MakeGenericMethod(typeof(int), typeof(string), typeof(int));
                        methodNew.Invoke(oGeneric, new object[] { 1328, "HEYHO", 1328 });
                    }
                }
                #endregion

                #region Reflection Property  O/RM
                {
                    //Console.WriteLine("************************Reflection*****************");
                    //{
                    //    People people = new People();
                    //    people.Id = 1387;
                    //    people.Name = "张先生";
                    //    people.Description = "做笔记的张先生";

                    //    Console.WriteLine($"people.Id={people.Id}");
                    //    Console.WriteLine($"people.Name={people.Name}");
                    //    Console.WriteLine($"people.Description={people.Description}");
                    //}
                    //{
                    //    Type type = typeof(People);
                    //    object oPeople = Activator.CreateInstance(type);
                    //    foreach (var item in type.GetProperties())//反射可以给对象的属性 动态 赋值/获取值，
                    //    {
                    //        Console.WriteLine(item.Name);
                    //        Console.WriteLine(item.GetValue(oPeople));
                    //        if (item.Name.Equals("Id"))
                    //            item.SetValue(oPeople, 1235);
                    //        else if (item.Name.Equals("Name"))
                    //            item.SetValue(oPeople, "快乐泥巴");
                    //        Console.WriteLine(item.GetValue(oPeople));
                    //    }
                    //    foreach (var item in type.GetFields())//反射可以给对象的字段  动态 赋值/获取值，
                    //    {
                    //        Console.WriteLine(item.Name);
                    //        Console.WriteLine(item.GetValue(oPeople));
                    //        if (item.Name.Equals("Description"))
                    //            item.SetValue(oPeople, "快乐的快乐泥巴");
                    //        Console.WriteLine(item.GetValue(oPeople));
                    //    }
                    //    //类型转换 
                    //    //一个方法完成多张表的查询  其实就是多个类型  泛型
                    //    //用户表查询：id     user的sql  datareader  new一个User，   绑定字段，数据来自datareader
                    //    //公司表查询：id  company的sql  datareader  new一个Company，绑定字段，数据来自datareader
                    //    //。。。。。
                    //}

                }
                #endregion

                Monitor.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

        private static T Find<T>(int id)
        {
            return default(T);
        }
    }
}
