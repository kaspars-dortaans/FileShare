export const DownloadFile = function (file: Blob, name: string) {
  const aElement = document.createElement('a')
  aElement.download = name
  aElement.href = window.URL.createObjectURL(file)
  aElement.click()
  window.URL.revokeObjectURL(aElement.href)
  aElement.remove()
}

export const GetBlobUrlData = function (blob: Blob) {
  return new Promise((resolve: (value: string) => void, reject) => {
    const reader = new FileReader()
    reader.onload = () => resolve(reader.result as string)
    reader.onerror = reject
    reader.readAsDataURL(blob)
  })
}
