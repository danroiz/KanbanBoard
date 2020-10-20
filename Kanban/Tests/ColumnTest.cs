
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace Tests
{
    class ColumnTest
    {
        const int DEFAULT_BOARD = 0;
        const int DEFAULT_LIMIT = 100;
        const string DEFAULT_MESSAGE = "boring error msg";
        const int LIMITLESS = -1;

        Column col0;
        Column col1;

        Mock<ColumnDTO> DTOCol0Mock;
        Mock<ColumnDTO> DTOCol1Mock;

        Mock<TaskDTO> DTOTsk0Mock;
        Mock<TaskDTO> DTOTsk1Mock;
        Mock<TaskDTO> DTOTsk2Mock;

        Mock<Task> task0;
        Mock<Task> task1;
        Mock<Task> task2;
        Exception exception;




        [SetUp]
        public void SetUp()
        {
            DTOCol0Mock = new Mock<ColumnDTO>("name@email.com", "col0", 0, DEFAULT_LIMIT, DEFAULT_BOARD);
            DTOCol0Mock.Setup(m => m.Name).Returns("col0");
            DTOCol0Mock.Setup(m => m.Limit).Returns(DEFAULT_LIMIT);
            DTOCol0Mock.Setup(m => m.ColumnOrdinal).Returns(0);

            DTOCol1Mock = new Mock<ColumnDTO>("name@email.com", "col1", 1, DEFAULT_LIMIT, DEFAULT_BOARD);
            DTOCol1Mock.Setup(m => m.Name).Returns("col1");
            DTOCol1Mock.Setup(m => m.Limit).Returns(DEFAULT_LIMIT);
            DTOCol1Mock.Setup(m => m.ColumnOrdinal).Returns(1);


            col0 = new Column(DTOCol0Mock.Object);
            col1 = new Column(DTOCol1Mock.Object);

            DTOTsk0Mock = new Mock<TaskDTO>("name@email.com", 0, 0, "Title", "Description", DateTime.Now.AddDays(1), DateTime.Now, 0, "name@email.com");
            task0 = new Mock<Task>(DTOTsk0Mock.Object);

            DTOTsk1Mock = new Mock<TaskDTO>("name@email.com", 0, 1, "Title", "Description", DateTime.Now.AddDays(1), DateTime.Now, 0, "name@email.com");
            task1 = new Mock<Task>(DTOTsk1Mock.Object);

            DTOTsk2Mock = new Mock<TaskDTO>("name@email.com", 0, 2, "Title", "Description", DateTime.Now.AddDays(1), DateTime.Now, 0, "name@email.com");
            task2 = new Mock<Task>(DTOTsk2Mock.Object);


            task0.Setup(m => m.Id).Returns(0);
            task0.Setup(m => m.ToDalObject()).Returns(DTOTsk0Mock.Object);


            task1.Setup(m => m.Id).Returns(1);
            task1.Setup(m => m.ToDalObject()).Returns(DTOTsk1Mock.Object);

            task2.Setup(m => m.Id).Returns(2);
            task2.Setup(m => m.ToDalObject()).Returns(DTOTsk2Mock.Object);

            DTOTsk0Mock.Setup(m => m.Insert());
            DTOTsk1Mock.Setup(m => m.Insert());
            DTOTsk2Mock.Setup(m => m.Insert());

            exception = new Exception(DEFAULT_MESSAGE);

        }

        [TearDown]
        public void Dispose()
        {
            col0 = null;
            col1 = null;

            DTOCol0Mock.Reset();
            DTOCol1Mock.Reset();

            DTOTsk0Mock.Reset();
            DTOTsk1Mock.Reset();
            DTOTsk2Mock.Reset();

            task0.Reset();
            task1.Reset();
            task2.Reset();

            exception = null;
        }
        // ========= THREE METHODS WE CHECK FOR 100% COVERAGE: AddTask, Merge, IsValidLimit ===============
        // ========= REST IS FOR OUR NAUGHTY PLEASURE
        //=========================Add Task Tests==================================
        [Test]
        public void AddTaskTest()
        {
            //Arrange
            SetUp();

            //Act
            try
            {
                col0.AddTask(task0.Object);

            }

            catch (Exception e)
            {
                exception = e;
            }

            //Assert                    
            Assert.That(exception.Message, Is.EqualTo(DEFAULT_MESSAGE));
        }

        [Test]
        public void AddTaskIdAllReadyExsist()
        {
            //arange
            SetUp();

            task1.Setup(m => m.Id).Returns(0);

            //act

            try
            {
                col0.AddTask(task0.Object);
                col0.AddTask(task1.Object);

            }
            catch (System.ArgumentException) // double key map exception
            {
                Assert.That(true);
            }

            Dispose();
        }

        [Test]
        public void AddTaskColumnFull_ThrowsException()
        {
            //arange
            SetUp();
            col0.Limit = 1;

            //act

            try
            {
                col0.AddTask(task0.Object);
                col0.AddTask(task1.Object);

            }
            catch (Exception e)
            {
                exception = e;
            }

            //Assert                    
            Assert.That(exception.Message, Is.EqualTo("Reached limit of tasks"));
            Dispose();
        }


        [Test]
        public void AddTaskColumnLimitLess()
        {
            //arange
            SetUp();
            col0.Limit = -1;

            try
            {

                //act

                col0.AddTask(task0.Object);
                col0.AddTask(task1.Object);

                //assert 
                Assert.AreEqual(col0.Tasks.Count, 2);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Dispose();
        }

        [Test]
        public void AddNullTaskFail()
        {
            //arange
            SetUp();



            try
            {
                //act
                col0.AddTask(null);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.AreEqual(exception.Message, "tried to add null task");
            Dispose();
        }



        //=========================Limit Tests==================================

        [Test]
        public void ValidLimitTest_LegalLimit_Success()
        {
            //arange
            SetUp();

            //act

            try
            {
                col0.IsValidLimit(col0.Tasks.Count + 1);
            }
            catch
            {
                Assert.Fail();
            }

            Dispose();
        }

        [Test]
        public void ValidLimitTest_MinimumLimit_Success()
        {
            //arange
            SetUp();
            col0.AddTask(task0.Object);
            col0.AddTask(task1.Object);
            col0.AddTask(task2.Object);

            //act
            try
            {
                col0.Limit = col0.Tasks.Count;
            }
            catch
            {
                Assert.Fail();
            }

            Dispose();
        }

        [Test]
        public void ValidLimitTest_TasksOverflow_Fail()
        {
            //arange
            SetUp();
            col0.AddTask(task0.Object);
            col0.AddTask(task1.Object);
            col0.AddTask(task2.Object);

            //act
            try
            {
                col0.IsValidLimit(col0.Tasks.Count - 1);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //assert
            Assert.That(exception.Message, Is.EqualTo("current task amount bigger than limit"));

            Dispose();
        }

        [Test]
        public void ValidLimitTest_NegativeLimit_Fail()
        {
            //arange
            SetUp();

            //act
            try
            {
                col0.IsValidLimit(-2);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //assert
            Assert.That(exception.Message, Is.EqualTo("current task amount bigger than limit"));

            Dispose();
        }

        [Test]
        public void ValidLimitTest_InfinityLimit_Success()
        {
            //arange
            SetUp();

            //act
            try
            {
                col0.IsValidLimit(LIMITLESS);
            }
            catch
            {
                Assert.Fail();
            }

            Dispose();
        }


        //=========================Merge Tests==================================

        [Test]
        public void MergeTest_ValidMerge_Success()
        {
            //arange
            SetUp();
            col1.AddTask(task0.Object);

            //act
            col0.Merge(col1, 0);

            //asert
            Assert.IsTrue(col0.Tasks.Contains(task0.Object));

            Dispose();
        }


        [Test]
        public void MergeTest_MergeToInifintyLimitColumn_Success()
        {
            //arange
            SetUp();
            col1.AddTask(task0.Object);
            col0.Limit = LIMITLESS;

            //act

            col0.Merge(col1, 0);

            //asert
            Assert.IsTrue(col0.Tasks.Contains(task0.Object));

            Dispose();


        }


        [Test]
        public void MergeTest_TasksOverflow_Fail()
        {
            //arange
            SetUp();
            col1.AddTask(task0.Object);
            col1.AddTask(task1.Object);
            col0.Limit = 1;

            //act
            try
            {
                col0.Merge(col1, 0);

            }
            catch (Exception e)
            {
                exception = e;
            }

            //assert
            Assert.That(exception.Message, Is.EqualTo("column will reach its limit by this merge"));

            Dispose();
        }

        [Test]
        public void MergeTest_NullColumn_Fail()
        {
            //arrange
            SetUp();

            //act
            try
            {
                col0.Merge(null, 0);
            }
            catch (Exception e)
            {
                exception = e;
            }

            //assert
            Assert.That(exception.Message, Is.EqualTo("tried to merge a null column"));

            Dispose();
        }

        [Test]
        public void MergeTest_CombineTasks_Success()
        {
            //arrange
            SetUp();
            col0.AddTask(task0.Object);
            col1.AddTask(task1.Object);

            //act
            col1.Merge(col0, 1);

            //assert
            Assert.IsTrue(col1.Tasks.Count == 2);

        }


        //=======================Valid Name Tests===============================

        [Test]
        public void NullNameTestFail()
        {
            //arange
            SetUp();
            try
            {
                col0.IsValidColumnName(null);
                Assert.Fail();
            }
            catch
            {
            }
            Dispose();
        }

        [Test]
        public void EmptyNameTestFail()
        {
            //arange
            SetUp();
            try
            {
                col0.IsValidColumnName("");
                Assert.Fail();
            }
            catch
            {
            }
            Dispose();
        }

        [Test]
        public void TooLongNameTest()
        {
            //arange
            SetUp();

            try
            {
                col0.IsValidColumnName("1234567890123456");
                Assert.Fail();
            }
            catch
            {
            }
            Dispose();
        }


        [Test]
        public void MaxLengthNameTestSuccess()
        {
            //arange
            SetUp();

            try
            {
                col0.IsValidColumnName("123456789012345");
            }
            catch
            {
                Assert.Fail();
            }
            Dispose();
        }


        //=======================Get Task Tests===============================

        [Test]
        public void GetTaskTestSuccess()
        {
            //arange
            SetUp();
            col0.AddTask(task0.Object); //not sure if it is ok to use other methodes


            Task taskReceived = col0.GetTask(0);
            Assert.AreEqual(task0.Object, taskReceived);


            Dispose();
        }


        [Test]
        public void GetTaskTeskIligalId()
        {
            //arange
            SetUp();
            col0.AddTask(task0.Object); //not sure if it is ok to use other methodes

            try
            {
                Task taskResived = col0.GetTask(1);
                Assert.Fail();
            }
            catch
            {
            }
            Dispose();
        }
    }
}
