namespace FraudSys.Domain.Services.Requests
{
    public class ExecutePixTransactionRequest
    {
        public required string SourceClientDocument { get; set; }
        public required int SourceClientAgency { get; set; }
        public required string SourceClientAccount { get; set; }
        public required string TargetClientDocument { get; set; }
        public required int TargetClientAgency { get; set; }
        public required string TargetClientAccount { get; set; }
        public required double TransactionAmount { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SourceClientDocument))
            {
                throw new ArgumentException("Documento do cliente de origem deve ser preenchido", nameof(SourceClientDocument));
            }

            if (SourceClientAgency <= 0)
            {
                throw new ArgumentException("Agência do cliente de origem deve deve maior que zero", nameof(SourceClientAgency));
            }

            if (string.IsNullOrWhiteSpace(SourceClientAccount))
            {
                throw new ArgumentException("Conta do cliente de origem deve ser preenchida", nameof(SourceClientAccount));
            }

            if (string.IsNullOrWhiteSpace(TargetClientDocument))
            {
                throw new ArgumentException("Documento do cliente de destino deve ser preenchido", nameof(TargetClientDocument));
            }

            if (TargetClientAgency <= 0)
            {
                throw new ArgumentException("Agência do cliente de destino deve deve maior que zero", nameof(TargetClientAgency));
            }

            if (string.IsNullOrWhiteSpace(TargetClientAccount))
            {
                throw new ArgumentException("Conta do cliente de destino deve ser preenchida", nameof(TargetClientAccount));
            }

            if (SourceClientDocument == TargetClientDocument && SourceClientAgency == TargetClientAgency && SourceClientAccount == TargetClientAccount)
            {
                throw new ArgumentException("Cliente de origem e destino não podem ser os mesmos", nameof(SourceClientDocument));
            }

            if (TransactionAmount <= 0)
            {
                throw new ArgumentException("Valor da transferência deve ser maior que zero", nameof(TransactionAmount));
            }
        }
    }
}
