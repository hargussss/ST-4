using Stateless;

namespace BugPro;

public class Bug
{
	public enum State
	{
		New,
		Triaged,
		InProgress,
		NeedInfo,
		Resolved,
		Closed,
		Reopened,
		Rejected
	}

	public enum Trigger
	{
		Triage,
		StartWork,
		AskForInfo,
		ProvideInfo,
		Resolve,
		Close,
		Reopen,
		Reject
	}

	private readonly StateMachine<State, Trigger> _machine;

	public Bug(State initialState = State.New)
	{
		_machine = new StateMachine<State, Trigger>(initialState);

		_machine.Configure(State.New)
			.Permit(Trigger.Triage, State.Triaged)
			.Permit(Trigger.Reject, State.Rejected);

		_machine.Configure(State.Triaged)
			.Permit(Trigger.StartWork, State.InProgress)
			.Permit(Trigger.AskForInfo, State.NeedInfo)
			.Permit(Trigger.Reject, State.Rejected);

		_machine.Configure(State.InProgress)
			.Permit(Trigger.Resolve, State.Resolved)
			.Permit(Trigger.AskForInfo, State.NeedInfo);

		_machine.Configure(State.NeedInfo)
			.Permit(Trigger.ProvideInfo, State.Triaged)
			.Permit(Trigger.Reject, State.Rejected);

		_machine.Configure(State.Resolved)
			.Permit(Trigger.Close, State.Closed)
			.Permit(Trigger.Reopen, State.Reopened);

		_machine.Configure(State.Closed)
			.Permit(Trigger.Reopen, State.Reopened);

		_machine.Configure(State.Reopened)
			.Permit(Trigger.StartWork, State.InProgress)
			.Permit(Trigger.Reject, State.Rejected);

		_machine.Configure(State.Rejected);
	}

	public State CurrentState => _machine.State;

	public IEnumerable<Trigger> GetPermittedTriggers() => _machine.PermittedTriggers;

	public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

	public bool IsFinalState => CurrentState is State.Closed or State.Rejected;

	public void Triage() => _machine.Fire(Trigger.Triage);
	public void StartWork() => _machine.Fire(Trigger.StartWork);
	public void AskForInfo() => _machine.Fire(Trigger.AskForInfo);
	public void ProvideInfo() => _machine.Fire(Trigger.ProvideInfo);
	public void Resolve() => _machine.Fire(Trigger.Resolve);
	public void Close() => _machine.Fire(Trigger.Close);
	public void Reopen() => _machine.Fire(Trigger.Reopen);
	public void Reject() => _machine.Fire(Trigger.Reject);
}

internal static class Program
{
	private static void Main()
	{
		var bug = new Bug();

		Console.WriteLine($"Initial state: {bug.CurrentState}");
		bug.Triage();
		Console.WriteLine($"After triage: {bug.CurrentState}");
		bug.StartWork();
		Console.WriteLine($"After start work: {bug.CurrentState}");
		bug.Resolve();
		Console.WriteLine($"After resolve: {bug.CurrentState}");
		bug.Close();
		Console.WriteLine($"After close: {bug.CurrentState}");
		Console.WriteLine($"Final state reached: {bug.IsFinalState}");
	}
}
