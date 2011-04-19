// <copyright file="DataReaderEnumerable.cs" company="Engage Software">
// Engage: Events
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumerates over an <see cref="IDataReader"/> instance, moving to the next record upon each enumeration.
    /// </summary>
    /// <remarks>
    /// Based on http://codecisions.com/post/2010/04/08/Enumerating-IDataReader-With-LINQ.aspx
    /// Stored on https://gist.github.com/873080
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "DataReaderCollection isn't really an accurate name")]
    public class DataReaderEnumerable : IEnumerable<IDataReader>, IDisposable
    {
        /// <summary>
        /// Whether the <see cref="DataReader"/> has been enumerated before
        /// </summary>
        private bool enumerated;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderEnumerable"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader to enumerate.</param>
        public DataReaderEnumerable(IDataReader dataReader)
        {
            this.DataReader = dataReader;
        }

        /// <summary>
        /// Gets the data reader being enumerated.
        /// </summary>
        public IDataReader DataReader { get; private set; }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="DataReader"/>.
        /// </summary>
        /// <returns>
        /// <returns>An <see cref="IEnumerable{T}"/> instance that can iterate over the rows in the <see cref="DataReader"/></returns>
        /// </returns>
        public IEnumerator<IDataReader> GetEnumerator()
        {
            if (this.enumerated)
            {
                throw new InvalidOperationException("The IDataReader can only be enumerated once.");
            }

            this.enumerated = true;
            return this.GetEnumeratorImpl();
        }

        /// <summary>
        /// Disposes the <see cref="DataReader"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Disposes the <see cref="DataReader"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.DataReader != null)
                {
                    this.DataReader.Dispose();
                    this.DataReader = null;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> instance that can iterate over the rows in the <see cref="DataReader"/>
        /// </returns>
        private IEnumerator<IDataReader> GetEnumeratorImpl()
        {
            using (this.DataReader)
            {
                while (this.DataReader.Read())
                {
                    yield return this.DataReader;
                }
            }
        }
    }
}