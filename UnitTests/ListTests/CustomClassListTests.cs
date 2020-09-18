// using NUnit.Framework;
// using JsonSrcGen;
// using System.Collections.Generic;
// using System;

// [assembly: JsonList(typeof(UnitTests.ListTests.CustomClass))] 


// namespace UnitTests.ListTests
// {
//     [Json]
//     public class CustomClass
//     {
//         public string Name {get;set;}
//     }

//     public class CustomClassListTests
//     { 
//         JsonSrcGen.JsonSrcGenConvert _convert;

//         string ExpectedJson = "[{\"Name\":\"William\"},null,{\"Name\":\"Susen\"}]";

//         [SetUp]
//         public void Setup()
//         {
//             _convert = new JsonSrcGenConvert();
//         }

//         [Test] 
//         public void ToJson_CorrectString()
//         {
//             //arrange
//             var list = new List<CustomClass>(){new CustomClass(){Name = "William"}, null, new CustomClass(){Name="Susen"}};

//             //act
//             var json = _convert.ToJson(list);

//             //assert
//             Assert.That(json, Is.EqualTo(ExpectedJson));
//         }

//         [Test]
//         public void ToJson_Null_CorrectString()
//         {
//             //arrange
//             //act
//             var json = _convert.ToJson((List<CustomClass>)null);

//             //assert
//             Assert.That(json, Is.EqualTo("null"));
//         }

//         [Test]
//         public void FromJson_EmptyList_CorrectList() 
//         {
//             //arrange
//             var list = new List<CustomClass>();

//             //act
//             _convert.FromJson(list, ExpectedJson);

//             //assert
//             Assert.That(list.Count, Is.EqualTo(3));
//             Assert.That(list[0].Name, Is.EqualTo("William"));
//             Assert.That(list[1], Is.Null);
//             Assert.That(list[2].Name, Is.EqualTo("Susen"));
//         }

//         [Test] 
//         public void FromJson_PopulatedList_CorrectList()
//         {
//             //arrange
//             var list = new List<CustomClass>(){new CustomClass(), new CustomClass(), new CustomClass()};

//             //act
//             list =_convert.FromJson(list, ExpectedJson);

//             //assert
//             Assert.That(list.Count, Is.EqualTo(3));
//             Assert.That(list[0].Name, Is.EqualTo("William"));
//             Assert.That(list[1], Is.Null);
//             Assert.That(list[2].Name, Is.EqualTo("Susen"));
//         }

//         [Test] 
//         public void FromJson_JsonNull_ReturnsNull()
//         {
//             //arrange
//             var list = new List<CustomClass>(){new CustomClass(), new CustomClass(), new CustomClass()};

//             //act
//             list = _convert.FromJson(list, "null");

//             //assert
//             Assert.That(list, Is.Null);
//         }

//         [Test] 
//         public void FromJson_ListNull_MakesList()
//         {
//             //arrange
//             //act
//             var list = _convert.FromJson((List<CustomClass>)null, ExpectedJson);

//             //assert
//             Assert.That(list.Count, Is.EqualTo(3));
//             Assert.That(list[0].Name, Is.EqualTo("William"));
//             Assert.That(list[1], Is.Null);
//             Assert.That(list[2].Name, Is.EqualTo("Susen"));
//         }
//     }
// }