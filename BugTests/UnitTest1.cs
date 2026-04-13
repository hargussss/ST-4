using BugPro;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void InitialState_IsNew()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.New, bug.CurrentState);
    }

    [TestMethod]
    public void Triage_FromNew_ToTriaged()
    {
        var bug = new Bug();
        bug.Triage();
        Assert.AreEqual(Bug.State.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromNew_ToRejected()
    {
        var bug = new Bug();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void StartWork_FromTriaged_ToInProgress()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void AskForInfo_FromTriaged_ToNeedInfo()
    {
        var bug = new Bug();
        bug.Triage();
        bug.AskForInfo();
        Assert.AreEqual(Bug.State.NeedInfo, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromTriaged_ToRejected()
    {
        var bug = new Bug();
        bug.Triage();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void Resolve_FromInProgress_ToResolved()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void AskForInfo_FromInProgress_ToNeedInfo()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.AskForInfo();
        Assert.AreEqual(Bug.State.NeedInfo, bug.CurrentState);
    }

    [TestMethod]
    public void ProvideInfo_FromNeedInfo_ToTriaged()
    {
        var bug = new Bug();
        bug.Triage();
        bug.AskForInfo();
        bug.ProvideInfo();
        Assert.AreEqual(Bug.State.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromNeedInfo_ToRejected()
    {
        var bug = new Bug();
        bug.Triage();
        bug.AskForInfo();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromResolved_ToClosed()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromResolved_ToReopened()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromClosed_ToReopened()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void StartWork_FromReopened_ToInProgress()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Reopen();
        bug.StartWork();
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromReopened_ToRejected()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Reopen();
        bug.Reject();
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void CanFire_Triage_FromNew_IsTrue()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(Bug.Trigger.Triage));
    }

    [TestMethod]
    public void CanFire_Resolve_FromNew_IsFalse()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanFire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void PermittedTriggers_FromNew_ContainsTriageAndReject()
    {
        var bug = new Bug();
        var permitted = bug.GetPermittedTriggers().ToList();

        CollectionAssert.Contains(permitted, Bug.Trigger.Triage);
        CollectionAssert.Contains(permitted, Bug.Trigger.Reject);
        Assert.AreEqual(2, permitted.Count);
    }

    [TestMethod]
    public void IsFinalState_ForClosed_IsTrue()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        bug.Resolve();
        bug.Close();

        Assert.IsTrue(bug.IsFinalState);
    }

    [TestMethod]
    public void IsFinalState_ForRejected_IsTrue()
    {
        var bug = new Bug();
        bug.Reject();
        Assert.IsTrue(bug.IsFinalState);
    }

    [TestMethod]
    public void IsFinalState_ForInProgress_IsFalse()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        Assert.IsFalse(bug.IsFinalState);
    }

    [TestMethod]
    public void InvalidTransition_Resolve_FromNew_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Resolve());
    }

    [TestMethod]
    public void InvalidTransition_Close_FromInProgress_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.Triage();
        bug.StartWork();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
    }

    [TestMethod]
    public void InvalidTransition_StartWork_FromRejected_ThrowsInvalidOperationException()
    {
        var bug = new Bug();
        bug.Reject();
        Assert.ThrowsException<InvalidOperationException>(() => bug.StartWork());
    }
}