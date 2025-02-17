﻿// See https://aka.ms/new-console-template for more information

namespace ZeUnit;

public interface IZeReporter
{
    void Report(ZeTest test, IEnumerable<ZeResult> result);
}