namespace SmartDose.WcfLib
{
    public enum ServiceResultStatus
    {
        #region Ok, OK
        Ok = 0,
        #endregion

        #region Ok But
        OkButDataIsNull = 10,
        OkButItemNotFound = 11,
        OkButListIsEmpty = 12,
        #endregion

        #region Error
        Error = -1,
        ErrorException = -2,
        ErrorConnection = -3,
        ErrorNoEnumForInt = -4,
        ErrorInvalidateArgs = -10,
        ErrorCouldNotDelete = -11,
        ErrorIdentifierNotFound = -12,
        ErrorNothingFound = -13,
        ErrorFailed = -14,
        #endregion
    }

   
}
