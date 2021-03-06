﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNSharp.DataTypes;
using static NNSharp.DataTypes.Data2D;
using NNSharp.SequentialBased.SequentialLayers;
using NNSharp.IO;
using NNSharp.Models;
using UnitTests.Properties;

namespace UnitTests
{
    [TestClass]
    public class TestGlobalAvgPool1D
    {
        [TestMethod]
        public void Test_GlobalAvgPool1D_Execute()
        {
            // Initialize data.
            Data2D data = new Data2D(1, 3, 2, 1);
            data[0, 0, 0, 0] = 1;
            data[0, 1, 0, 0] = 2;
            data[0, 2, 0, 0] = 0;

            data[0, 0, 1, 0] = 3;
            data[0, 1, 1, 0] = 4;
            data[0, 2, 1, 0] = 0;

            GlobalAvgPool1DLayer pool = new GlobalAvgPool1DLayer();
            pool.SetInput(data);
            pool.Execute();
            Data2D output = pool.GetOutput() as Data2D;

            // Checking sizes
            Dimension dim = output.GetDimension();
            Assert.AreEqual(dim.b, 1);
            Assert.AreEqual(dim.c, 2);
            Assert.AreEqual(dim.h, 1);
            Assert.AreEqual(dim.w, 1);

            // Checking calculation
            Assert.AreEqual(output[0, 0, 0, 0], 1.0, 0.0000001);
            Assert.AreEqual(output[0, 0, 1, 0], 7.0/3.0, 0.0000001);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void Test_GlobalAvgPool1D_Null_Input()
        {
            Data2D data = null;
            GlobalAvgPool1DLayer pool = new GlobalAvgPool1DLayer();
            pool.SetInput(data);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void Test_GlobalAvgPool1D_DifferentData_Input()
        {
            DataArray data = new DataArray(5);
            GlobalAvgPool1DLayer pool = new GlobalAvgPool1DLayer();
            pool.SetInput(data);
        }

        [TestMethod]
        public void Test_GlobalAvgPool1D_KerasModel()
        {
            string pathModel = Resources.TestsFolder + "test_globalavgpool_1D_model.json";
            string pathInput = Resources.TestsFolder + "test_globalavgpool_1D_input.json";
            string pathOutput = Resources.TestsFolder + "test_globalavgpool_1D_output.json";

            Utils.KerasModelTest(pathInput, pathModel, pathOutput);
        }
    }
}
