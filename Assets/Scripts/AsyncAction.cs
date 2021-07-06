using System.Threading.Tasks;

public delegate Task AsyncAction();
public delegate Task AsyncAction<in T1>(T1 arg1);
public delegate Task AsyncAction<in T1, in T2>(T1 arg1, T2 arg2);
