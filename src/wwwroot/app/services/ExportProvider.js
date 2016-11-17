const EXPORT_URL = 'export';
let serverUrl;

let _$serializer;

export default class ExportProvider {
   changeApiUrl(url) {
      serverUrl = url;
   }

   $get($httpParamSerializer) {
      'ngInject';
      _$serializer = $httpParamSerializer;
      return new ExportService();
   }
}

class ExportService {
   exportCandidateProgressReport(data) {
      let param = _$serializer(data);
      return `${serverUrl}${EXPORT_URL}/candidateProgressReport?${param}`;
   }
}
