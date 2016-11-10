import {
   last,
   cloneDeep,
   assign
} from 'lodash';
let _$state, _transitionsHistory;
const DEFAULT_BACK_TO = 'home';
export default class TransitionsService {
   constructor($state) {
      _$state             = $state;
      _transitionsHistory = [];
   }

   /**
   * API for transition with saving state params to transitions history for return  to this state.
   * @param {string}   name  - name of state to go.
   * @param {Object}   params - object for params
   * @param {Object}   data - object which will be assigned to current state params
   * @returns {promise} will be resolved when transition is successful */
   go(name, params, data) {
      return _$state.go(name, params).then(() => {
         _transitionsHistory.push({ name   : _$state.previous.name,
                                    params : _attach(_$state.previous.params, data) });
      });
   }

   /**
   * API for transition to previous state or specific state with cleaning last step in history.
   * @param {string}   backTo  - name of state to back if history is empty or need back force to specific state.
   * @param {Object}   params - object for params
   * @returns {promise} will be resolved when transition is successful */
   back(backTo, params) {
      params = params || {};
      if (!_transitionsHistory.length) {
         return _$state.go(backTo || DEFAULT_BACK_TO, params);
      }
      let stateInfo = last(_transitionsHistory);
      return _$state.go(backTo || stateInfo.name, _attach(stateInfo.params, params))
         .then(() => _transitionsHistory.pop());
   }
}

function _attach(params, data) {
   return assign(cloneDeep(params), data);
}
