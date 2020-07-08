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
        <form>
          <div class="form-group">
            <label for="searchUsername">Username</label>
            <input type="text" class="form-control" id="searchUsername" placeholder="Search contact by username" v-model="searchTerm">
          </div>
        </form>
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
import mixins from '../mixins.js'

export default {
  name: 'searchmodal',
  props: ['primaryColor'],
  mixins: [mixins],
  components: {
  },
  data () {
    return {
      color: this.primaryColor,
      searchTerm: null
    }
  },
  mounted () {
  },
  methods: {
    searchContactByName: function () {
      // eslint-disable-next-line
      axios.get(`${this.apiUrl}api/user/searchContactByName/${this.userId}/${this.searchTerm}`)
        .then(function (response) {
        }).catch(error => {
          console.log(error.response)
        })
    }
  },
  watch: {
    searchTerm: function () {
      if (this.searchTerm) {
        // this.searchContactByName()
      }
    }
  }
}
</script>

<style>
</style>
