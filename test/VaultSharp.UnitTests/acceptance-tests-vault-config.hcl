backend "file" {
  path = "##FILE_BACKEND_PATH##"
  }

listener "tcp" {
  address = "127.0.0.1:8200"
  tls_disable = 1
}