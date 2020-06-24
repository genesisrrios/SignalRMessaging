<template>
  <div class="row">
    <div class="col-sm"></div>
    <div class="col-sm">
      <form>
        <div class="form-group">
          <label for="exampleInputEmail1">Username</label>
          <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Username">
        </div>
        <div class="form-group">
          <label for="exampleInputPassword1">Password</label>
          <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">
          <small>Don't have an account? <a style="cursor:pointer">Create one</a></small>
        </div>
        <button class="btn btn-primary">Log in</button>
      </form>
    </div>
    <div class="col-sm"></div>
  </div>
</template>

<script>
import mixins from '../mixins.js'

export default {
  name: 'home',
  mixins: [mixins],
  data () {
    return {
    }
  },
  methods: {
  },
  created: function () {
  }
}

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
