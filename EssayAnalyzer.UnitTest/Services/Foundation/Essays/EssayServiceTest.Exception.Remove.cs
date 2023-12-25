﻿using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using FluentAssertions;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someEssayId = Guid.NewGuid();
        SqlException sqlException = CreateSqlException();

        var failedEssayStorageException =
            new FailedEssayStorageException(sqlException);

        var expectedEssayDependencyException =
            new EssayDependencyException(failedEssayStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectEssayByIdAsync(someEssayId)).Throws(sqlException);

        // when
        ValueTask<Essay> removeEssayTask = this.essayService.RemoveEssayByIdAsync(someEssayId);

        EssayDependencyException actualEssayDependencyException =
            await Assert.ThrowsAsync<EssayDependencyException>(removeEssayTask.AsTask);

        // then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayDependencyException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedEssayDependencyException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.DeleteEssayAsync(It.IsAny<Essay>()), Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}