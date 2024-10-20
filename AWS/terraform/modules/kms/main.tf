resource "aws_kms_key" "poseidon_kms" {
  description = "KMS key for Poseidon project"
  policy      = data.aws_iam_policy_document.kms_policy.json
}

data "aws_iam_policy_document" "kms_policy" {
  statement {
    actions = ["kms:*"]
    principals {
      type        = "AWS"
      identifiers = ["*"]
    }
    resources = ["*"]
  }
}

resource "aws_kms_alias" "poseidon_kms_alias" {
  name          = "alias/poseidon-kms"
  target_key_id = aws_kms_key.poseidon_kms.id
}
