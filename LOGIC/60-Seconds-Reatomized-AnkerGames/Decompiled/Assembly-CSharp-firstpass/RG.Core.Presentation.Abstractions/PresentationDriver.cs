using RG.Core.Flow.Abstractions;

namespace RG.Core.Presentation.Abstractions;

public abstract class PresentationDriver<DATA, VIEW> : Driver<DATA, VIEW> where DATA : class where VIEW : class
{
}
