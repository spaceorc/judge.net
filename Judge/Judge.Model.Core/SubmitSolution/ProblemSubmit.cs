namespace Judge.Model.Core.SubmitSolution
{
    public sealed class ProblemSubmit : SubmitBase
    {
        public static ProblemSubmit Create()
        {
            var submit = new ProblemSubmit();
            submit.Results.Add(new SubmitResult(submit));
            return submit;
        }
    }
}
