using BasePoint.Core.Cqrs.Dapper.UnitOfWork;
using Moq;
using System.Data;
using Xunit;

namespace BasePoint.Core.Cqrs.Dapper.Tests.UnitOfWork
{
    public class DapperUnitOfWorkTests
    {
        private readonly Mock<IDbConnection> _connection;
        private readonly DapperUnitOfWork _unitOfWork;
        public DapperUnitOfWorkTests()
        {
            _connection = new Mock<IDbConnection>();

            _unitOfWork = new DapperUnitOfWork(_connection.Object);
        }

        [Fact]
        public void Dispose_Always_CallingGC()
        {
            _unitOfWork.Dispose();
        }

        [Fact]
        public async Task BeforeSave_ConnectionIsClosed_CallOpenMethod()
        {
            await _unitOfWork.BeforeSaveAsync();

            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            _connection.Verify(x => x.Open(), Times.Once);
        }

        [Fact]
        public async Task AfterSave_WhenParameterSuccessIsTrue_CallTransactionCommit()
        {
            //Arrange
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterSave(true);

            //Assert
            _connection.Verify(x => x.Open(), Times.Once);
        }

        [Fact]
        public async Task AfterSave_WhenParameterSuccessIsFalse_ShouldNotCallTransactionCommit()
        {
            //Arrange
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterSave(false);

            //Assert
            _connection.Verify(x => x.Open(), Times.Once);
        }

        [Fact]
        public async Task AfterRollBack_Always_ShouldCallTransactionRollback()
        {
            //Arrange
            _connection.Setup(x => x.State).Returns(ConnectionState.Closed);

            await _unitOfWork.BeforeSaveAsync();

            //Act
            await _unitOfWork.AfterRollBackAsync();

            //Assert
        }
    }
}
