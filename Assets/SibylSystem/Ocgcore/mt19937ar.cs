#region "License"

/* 
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto.

   Before using, initialize the state by using init_genrand(seed)  
   or init_by_array(init_key, key_length).

   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          
   Copyright (C) 2005, Mutsuo Saito,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

     1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.

     2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

     3. The names of its contributors may not be used to endorse or promote 
        products derived from this software without specific prior written 
        permission.
*/

/*
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


   Any feedback is very welcome.
   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
*/

/* 
   A C#-program for MT19937, with initialization improved 2006/01/06.
   Coded by Mitil.

   Copyright (C) 2006, Mitil, All rights reserved.

   Any feedback is very welcome.
   URL: http://meisui.psk.jp/
   email: m-i-t-i-l [at@at] p-s-k . j-p
           (remove dash[-], and replace [at@at] --> @)
*/

#endregion

using System;
using System.Security.Cryptography;

namespace Meisui.Random
{
    public class MersenneTwister
    {
        #region "Destructor"

        ~MersenneTwister()
        {
            mt = null;
            mag01 = null;
        }

        #endregion

        #region "Get Unsigned Int 32bit number"

        /* generates a random number on [0,0xffffffff]-Interval */
        public uint genrand_Int32()
        {
            uint y;

            if (mti >= N)
            {
                /* generate N words at one time */
                short kk;

                if (mti == N + 1) /* if init_genrand() has not been called, */
                    init_genrand(5489); /* a default initial seed is used */

                for (kk = 0; kk < N - M; kk++)
                {
                    y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
                    mt[kk] = mt[kk + M] ^ mag01[mt[kk + 1] & 1] ^ y;
                }

                for (; kk < N - 1; kk++)
                {
                    y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
                    mt[kk] = mt[kk + (M - N)] ^ mag01[mt[kk + 1] & 1] ^ y;
                }

                y = ((mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK)) >> 1;
                mt[N - 1] = mt[M - 1] ^ mag01[mt[0] & 1] ^ y;

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y ^= y >> 11;
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= y >> 18;

            return y;
        }

        #endregion

        #region "Get Int31 number"

        /* generates a random number on [0,0x7fffffff]-Interval */
        public uint genrand_Int31()
        {
            return genrand_Int32() >> 1;
        }

        #endregion

        #region "Private Parameter"

        /* Period parameters */
        private const short N = 624;
        private const short M = 397;
        private const uint MATRIX_A = 0x9908b0df; /* constant vector a */
        private const uint UPPER_MASK = 0x80000000; /* most significant w-r bits */
        private const uint LOWER_MASK = 0x7fffffff; /* least significant r bits */
        private uint[] mt; /* the array for the state vector  */
        private ushort mti; /* mti==N+1 means mt[N] is not initialized */
        private uint[] mag01;

        #endregion

        #region "Constructor"

        public MersenneTwister(uint s)
        {
            MT();
            init_genrand(s);
        }

        // coded by Mitil. 2006/01/04
        public MersenneTwister()
        {
            MT();

            // auto generate seed for .NET
            var seed_key = new uint[6];
            var rnseed = new byte[8];

            seed_key[0] = (uint) DateTime.Now.Millisecond;
            seed_key[1] = (uint) DateTime.Now.Second;
            seed_key[2] = (uint) DateTime.Now.DayOfYear;
            seed_key[3] = (uint) DateTime.Now.Year;
            ;
            RandomNumberGenerator rn
                = new RNGCryptoServiceProvider();
            rn.GetNonZeroBytes(rnseed);

            seed_key[4] = ((uint) rnseed[0] << 24) | ((uint) rnseed[1] << 16)
                                                   | ((uint) rnseed[2] << 8) | rnseed[3];
            seed_key[5] = ((uint) rnseed[4] << 24) | ((uint) rnseed[5] << 16)
                                                   | ((uint) rnseed[6] << 8) | rnseed[7];

            init_by_array(seed_key);

            rn = null;
            seed_key = null;
            rnseed = null;
        }

        public MersenneTwister(uint[] init_key)
        {
            MT();

            init_by_array(init_key);
        }

        private void MT()
        {
            mt = new uint[N];

            mag01 = new uint[] {0, MATRIX_A};
            /* mag01[x] = x * MATRIX_A  for x=0,1 */

            mti = N + 1;
        }

        #endregion

        #region "seed init"

        /* initializes mt[N] with a seed */
        private void init_genrand(uint s)
        {
            mt[0] = s;

            for (mti = 1; mti < N; mti++)
                mt[mti] =
                    1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti;
            /* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
            /* In the previous versions, MSBs of the seed affect   */
            /* only MSBs of the array mt[].                        */
            /* 2002/01/09 modified by Makoto Matsumoto             */
        }

        /* initialize by an array with array-length */
        /* init_key is the array for initializing keys */
        /* key_length is its length */
        /* slight change for C++, 2004/2/26 */
        private void init_by_array(uint[] init_key)
        {
            uint i, j;
            int k;
            var key_length = init_key.Length;

            init_genrand(19650218);
            i = 1;
            j = 0;
            k = N > key_length ? N : key_length;

            for (; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525))
                        + init_key[j] + j; /* non linear */
                i++;
                j++;
                if (i >= N)
                {
                    mt[0] = mt[N - 1];
                    i = 1;
                }

                if (j >= key_length) j = 0;
            }

            for (k = N - 1; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941))
                        - i; /* non linear */
                i++;
                if (i >= N)
                {
                    mt[0] = mt[N - 1];
                    i = 1;
                }
            }

            mt[0] = 0x80000000; /* MSB is 1; assuring non-zero initial array */
        }

        #endregion

        #region "Get type'double' number"

        /* generates a random number on [0,1]-real-Interval */
        public double genrand_real1()
        {
            return genrand_Int32() * (1.0 / 4294967295.0);
            /* divided by 2^32-1 */
        }

        /* generates a random number on [0,1)-real-Interval */
        public double genrand_real2()
        {
            return genrand_Int32() * (1.0 / 4294967296.0);
            /* divided by 2^32 */
        }

        /* generates a random number on (0,1)-real-Interval */
        public double genrand_real3()
        {
            return (genrand_Int32() + 0.5) * (1.0 / 4294967296.0);
            /* divided by 2^32 */
        }

        /* generates a random number on [0,1) with 53-bit resolution*/
        public double genrand_res53()
        {
            uint a = genrand_Int32() >> 5, b = genrand_Int32() >> 6;
            return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
        }

        /* These real versions are due to Isaku Wada, 2002/01/09 added */

        #endregion
    }
}