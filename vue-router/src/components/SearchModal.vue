<template>
<!-- Modal -->
<div class="modal fade" id="modalComponent" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div v-bind:style="{'background-color':primaryColor}"  class="modal-content">
      <div class="modal-header" v-bind:style="{'background-color':primaryColor}">
        <h5 class="modal-title" id="exampleModalLabel">Search username</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
      </div>
      <div class="modal-body">
        <select id="ajax-select2" class="js-data-example-ajax form-control"></select>
      </div>
      <div class="modal-footer" v-bind:style="{'background-color':primaryColor}">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary">Save changes</button>
      </div>
    </div>
  </div>
 </div>
</template>

<script>
// eslint-disable-next-line
import select2 from 'select2'
import jQuery from 'jquery'
import mixins from '../mixins.js'
let apiUrl
const $ = jQuery
window.$ = $

$('#ajax-select2').select2({
  ajax: {
    url: `${apiUrl}}`,
    data: function (params) {
      var query = {
        search: params.term,
        type: 'public'
      }

      // Query parameters will be ?search=[term]&type=public
      return query
    }
  }
})
export default {
  name: 'searchmodal',
  props: ['primaryColor'],
  mixins: [mixins],
  components: {
  },
  data () {
    return {
      color: this.primaryColor
    }
  },
  mounted () {
    apiUrl = this.apiUrl
  }
}
</script>

<style>
</style>
