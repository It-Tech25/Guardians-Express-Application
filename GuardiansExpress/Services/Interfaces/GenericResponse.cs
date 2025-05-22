namespace GuardiansExpress.Utilities
{
	public class GenericResponse
	{
		public string message { get; set; }
        public string Message { get; internal set; }
        public int statuCode { get; set; }
		public int currentId { get; set; }
		public string ErrorMessage { get; set; }
        public bool Success { get; internal set; }
    }
}
	