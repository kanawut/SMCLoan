﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace SMCLoanFacts
{
    public class StringFacts
    {
        
        public class LengthProperty
        {
            [Fact]
            public void EmptyString_ShouldHaveZeroLength()
            {

                //arrange
                string a = string.Empty;

                //act
                int n = a.Length;

                //assert
                Assert.Equal(0, n);
            }
        }
    }
}
